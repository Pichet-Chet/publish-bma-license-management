﻿
namespace bma_license_repository.CustomModel.SysConfiguration
{
    public class GetSysConfiguration
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
