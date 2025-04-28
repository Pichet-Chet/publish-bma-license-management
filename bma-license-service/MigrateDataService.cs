using System.ComponentModel;
using bma_license_repository;
using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Response.MigrateData;
using bma_license_service.Helper;
using Microsoft.AspNetCore.Http;

namespace bma_license_service
{
    public interface IMigrateDataService
    {
        Task<MigrateDataResponse> Orgranize(IFormFile? fileUpload);

        Task<MigrateDataResponse> License(IFormFile? fileUpload);
    }

    public class MigrateDataService : IMigrateDataService
    {
        private readonly IMigrateDataRepository _repository;
        private readonly ITransKeyRepository _transKeyRepository;
        private readonly ISysOrgranizeRepository _sysOrgranizeRepository;
        private readonly DateTime _datetime;
        private readonly ISysKeyTypeRepository _sysKeyTypeRepository;
        private readonly Guid _figCreated = new Guid();
        private readonly Guid _figKeyType = new Guid();
        private readonly ITransEquipmentRepository _transEquipmentRepository;

        public MigrateDataService(IMigrateDataRepository migrateDataRepository,
            ITransKeyRepository transKeyRepository,
            ISysOrgranizeRepository sysOrgranizeRepository,
            ISysKeyTypeRepository sysKeyTypeRepository,
            ITransEquipmentRepository transEquipmentRepository)
        {
            _repository = migrateDataRepository;
            _transKeyRepository = transKeyRepository;
            _sysOrgranizeRepository = sysOrgranizeRepository;
            _datetime = DateTime.Now;
            _sysKeyTypeRepository = sysKeyTypeRepository;
            _figCreated = "4d143f87-6228-4afe-ad15-aedcf5741eaf".ToGuid();
            _figKeyType = "40c1f1ea-da6d-495d-927b-4c18cdd99b28".ToGuid();
            _transEquipmentRepository = transEquipmentRepository;
        }

        public async Task<MigrateDataResponse> Orgranize(IFormFile? fileUpload)
        {
            MigrateDataResponse result = new MigrateDataResponse();

            try
            {
                result = await _repository.Orgranize(fileUpload);
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<MigrateDataResponse> License(IFormFile? fileUpload)
        {
            MigrateDataResponse result = new MigrateDataResponse();

            try
            {
                string uploads = Directory.GetCurrentDirectory() + "/" + "upload/migrate/";

                string pathUpload = "";

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                if (fileUpload.Length > 0)
                {
                    var fileName = fileUpload.FileName;
                    string filePath = Path.Combine(uploads, fileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(fileStream);
                    }
                    pathUpload = filePath;
                }

                var getOrgranize = await _sysOrgranizeRepository.GetAll();
                var dtTable = pathUpload.ReadToExcel();
                var getTransKey = await _sysKeyTypeRepository.GetAll(false);

                var count = dtTable.Rows.Count;

                result.Logging = new List<Logging>();

                int start = 1;

                List<string> licenses = new List<string>();

                for (int i = start; i < dtTable.Rows.Count; i++)
                {
                    //string code = dtTable.Rows[i][8].ToString();
                    string license = dtTable.Rows[i][3].ToString().Trim();

                    licenses.Add(license);
                }

                int distinctCount = licenses.Distinct().Count();


                for (int i = start; i < dtTable.Rows.Count; i++)
                {
                    string code = dtTable.Rows[i][8].ToString();
                    string license = dtTable.Rows[i][3].ToString();
                    string orgranizeFullName = dtTable.Rows[i][2].ToString();
                    string remark = dtTable.Rows[i][6].ToString();
                    string equipmentCode = dtTable.Rows[i][5].ToString();

                    if (code.Length != 8 && !string.IsNullOrEmpty(code))
                    {
                        result.Logging.Add(new Logging
                        {
                            Code = code,
                            Name = "Code invalid. Letters are not equal to 8 letters.",
                            Row = i
                        });
                    }
                    else if (!code.IsAllDigits() && !string.IsNullOrEmpty(code))
                    {
                        result.Logging.Add(new Logging
                        {
                            Code = code,
                            Name = "Code invalid. The string contains non-numeric characters.",
                            Row = i
                        });
                    }
                    else
                    {
                        var (departmentCode, divisionCode, sectionCode, jobCode) = code.GetCodeFromDataImport(false);

                        var findOrgranize = await _sysOrgranizeRepository.GetOrgranize(departmentCode, divisionCode, sectionCode, jobCode);

                        //var findOrgranize = getOrgranize.Where(a =>
                        //a.DepartmentCode == departmentCode &&
                        //a.DivisionCode == divisionCode &&
                        //a.SectionCode == sectionCode &&
                        //a.JobCode == jobCode).FirstOrDefault();

                        if (findOrgranize == null && !string.IsNullOrEmpty(code))
                        {
                            string nameOrgranize = await ExtractTextOutsideBrackets(orgranizeFullName);
                            string orgranizeType = await SetOrgranizeType(divisionCode, sectionCode, jobCode);
                            findOrgranize = await AddSysOrgranize(code, departmentCode, divisionCode, sectionCode, jobCode, nameOrgranize, _figCreated, orgranizeType);
                        }

                        Guid? orgranizeId = findOrgranize != null ? findOrgranize.Id : null;

                        var findLicense = await _transKeyRepository.GetForUpdate(license);

                        if(!string.IsNullOrEmpty(license))
                        {
                            if (findLicense == null)
                            {
                                findLicense = await AddTransKey(license, remark, orgranizeId);
                            }
                            else
                            {
                                findLicense = await UpdateTransKey(findLicense, license, remark, orgranizeId);
                            }
                        }

                        if(!string.IsNullOrEmpty(equipmentCode))
                        {
                            var findEquipment = await _transEquipmentRepository.GetForUpdate(equipmentCode);
                            if (findEquipment == null)
                            {
                                findEquipment = await AddTransEquipment(findLicense.Id, equipmentCode);
                            }
                            else
                            {
                                findEquipment = await UpdateTransEquipment(findEquipment, findLicense.Id, equipmentCode);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        private async Task<string> SetOrgranizeType(string divisionCode, string sectionCode, string jobCode)
        {
            string result = OrgranizeType.Department;

            if (divisionCode != OrgranizeType.Empty)
            {
                result = OrgranizeType.Division;
            }

            if (sectionCode != OrgranizeType.Empty)
            {
                result = OrgranizeType.Section;
            }

            if (jobCode != OrgranizeType.Empty)
            {
                result = OrgranizeType.Job;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = OrgranizeType.Department;
            }

            return result;
        }

        private async Task<string> ExtractTextOutsideBrackets(string input)
        {
            int closingBracketIndex = input.IndexOf("]");

            if (closingBracketIndex != -1 && closingBracketIndex + 1 < input.Length)
            {
                return input.Substring(closingBracketIndex + 1).Trim();
            }

            return input.Trim();
        }

        private async Task<GetSysOrgranize> AddSysOrgranize(string code, string departmentCode, string divisionCode, string sectionCode, string jobCode, string nameOrgranize, Guid createdBy, string typeName)
        {

            SysOrgranize sysOrgranize = new SysOrgranize
            {
                Code = code,
                CreatedBy = createdBy,
                CreatedDate = _datetime,
                DepartmentCode = departmentCode,
                DivisionCode = divisionCode,
                Id = HelperService.GenerateGuid(),
                IsActive = true,
                JobCode = jobCode,
                Name = nameOrgranize,
                SectionCode = sectionCode,
                UpdatedBy = createdBy,
                UpdatedDate = _datetime,
                TypeName = typeName
            };

            await _sysOrgranizeRepository.InsertAsync(sysOrgranize);

            return await _sysOrgranizeRepository.GetById(sysOrgranize.Id);
        }

        private async Task<TransKey> UpdateTransKey(TransKey transKey, string license, string remark, Guid? sysOrgranizeId)
        {
            transKey.License = license;
            transKey.UpdatedBy = _figCreated;
            transKey.UpdatedDate = _datetime;
            transKey.IsActive = true;
            transKey.Remark = remark;
            transKey.SysKeyTypeId = _figKeyType;
            transKey.SysOrgranizeId = sysOrgranizeId;

            await _transKeyRepository.UpdateAsync();

            return await _transKeyRepository.GetForUpdate(license);
        }

        private async Task<TransKey> AddTransKey(string license, string remark, Guid? sysOrgranizeId)
        {
            TransKey transKey = new TransKey
            {
                CreatedBy = _figCreated,
                CreatedDate = _datetime,
                Id = HelperService.GenerateGuid(),
                IsActive = true,
                License = license,
                Remark = remark,
                SysKeyTypeId = _figKeyType,
                SysOrgranizeId = sysOrgranizeId,
                UpdatedBy = _figCreated,
                UpdatedDate = _datetime,
            };

            await _transKeyRepository.InsertAsync(transKey);
            return transKey;
        }

        private async Task<TransEquipment> UpdateTransEquipment(TransEquipment transEquipment, Guid transKeyId, string equipmentCode)
        {
            transEquipment.TransKeyId = transKeyId;
            transEquipment.EquipmentCode = equipmentCode;
            transEquipment.IsActive = true;
            transEquipment.UpdatedDate = _datetime;
            transEquipment.UpdatedBy = _figCreated;

            await _transEquipmentRepository.UpdateAsync();
            return await _transEquipmentRepository.GetForUpdate(equipmentCode);
        }

        private async Task<TransEquipment> AddTransEquipment(Guid transKeyId, string equipmentCode)
        {
            TransEquipment transEquipment = new TransEquipment
            {
                CreatedBy = _figCreated,
                CreatedDate = _datetime,
                EquipmentCode = equipmentCode,
                Id = HelperService.GenerateGuid(),
                IsActive = true,
                TransKeyId = transKeyId,
                UpdatedBy = _figCreated,
                UpdatedDate = _datetime,
            };

            await _transEquipmentRepository.InsertAsync(transEquipment);
            return transEquipment;
        }
    }
}

