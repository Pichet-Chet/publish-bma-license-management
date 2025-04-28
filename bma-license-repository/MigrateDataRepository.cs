using System.Collections.Concurrent;
using System.Data;
using System.Reflection.Emit;
using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.CustomModel.TransKey;
using bma_license_repository.Dto;
using bma_license_repository.Helper;
using bma_license_repository.Response.MigrateData;
using Microsoft.AspNetCore.Http;

namespace bma_license_repository
{
    public interface IMigrateDataRepository
    {
        Task<MigrateDataResponse> Orgranize(IFormFile? fileUpload);


    }

    public class MigrateDataRepository : IMigrateDataRepository
    {
        private readonly DevBmaContext _context;

        public MigrateDataRepository(DevBmaContext context)
        {
            _context = context;
        }

        public async Task<MigrateDataResponse> Orgranize(IFormFile? fileUpload)
        {
            MigrateDataResponse result = new MigrateDataResponse();

            List<MigrateDataResponseData> data = new List<MigrateDataResponseData>();

            List<Logging> logError = new List<Logging>();

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

                if (!string.IsNullOrEmpty(pathUpload))
                {
                    var dtData = Helper.Helper.ReadExcelFileLicense(pathUpload);

                    int idx = 3;

                    foreach (var dr in dtData.AsEnumerable().Skip(2))
                    {
                        var newRecords = new SysOrgranize();

                        string orgranizeType = await SetOrgranizeType(dr["column3"].ToString(), dr["column4"].ToString(), dr["column5"].ToString());

                        SysOrgranize sysOrgranize = new SysOrgranize
                        {
                            Name = dr["column1"].ToString(),
                            DepartmentCode = dr["column2"].ToString(),
                            DivisionCode = dr["column3"].ToString(),
                            SectionCode = dr["column4"].ToString(),
                            JobCode = dr["column5"].ToString(),
                            Code = dr["column2"].ToString() + dr["column3"].ToString() + dr["column4"].ToString() + dr["column5"].ToString(),
                            TypeName = orgranizeType,
                            IsActive = true
                        };

                        var existingRecord = _context.SysOrgranizes.FirstOrDefault(x =>
                             x.DepartmentCode == sysOrgranize.DepartmentCode &&
                             x.DivisionCode == sysOrgranize.DivisionCode &&
                             x.SectionCode == sysOrgranize.SectionCode &&
                             x.JobCode == sysOrgranize.JobCode);

                        if (!string.IsNullOrEmpty(sysOrgranize.DepartmentCode) &&
                            !string.IsNullOrEmpty(sysOrgranize.DivisionCode) &&
                            !string.IsNullOrEmpty(sysOrgranize.SectionCode) &&
                            !string.IsNullOrEmpty(sysOrgranize.JobCode))
                        {
                            if (sysOrgranize.Code.Length == 8)
                            {
                                if (existingRecord == null)
                                {
                                    sysOrgranize.TypeName = orgranizeType;

                                    newRecords = sysOrgranize;

                                    _context.SysOrgranizes.Add(newRecords);

                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    existingRecord.Name = sysOrgranize.Name;
                                    existingRecord.DepartmentCode = sysOrgranize.DepartmentCode;
                                    existingRecord.DivisionCode = sysOrgranize.DivisionCode;
                                    existingRecord.SectionCode = sysOrgranize.SectionCode;
                                    existingRecord.JobCode = sysOrgranize.JobCode;
                                    existingRecord.Code = sysOrgranize.Code;
                                    existingRecord.IsActive = true;
                                    existingRecord.TypeName = orgranizeType;

                                    await _context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                Logging log = new Logging();

                                log.Row = idx;
                                log.Name = sysOrgranize.Name;
                                log.Code = sysOrgranize.Code;

                                logError.Add(log);
                            }
                        }

                        idx++;
                    }


                    if (logError.Count > 0)
                    {
                        result.MessageAlert.TH = "นำเข้าข้อมูลไม่สำเร็จ รูปแบบรหัสหน่วยงานไม่ถูกต้อง";
                        result.MessageAlert.EN = "Import Faild";

                        result.Logging = logError;
                    }
                    else
                    {
                        result.MessageAlert.TH = "นำเข้าข้อมูลสำเร็จ";
                        result.MessageAlert.EN = "Import Success";
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

    }
}

