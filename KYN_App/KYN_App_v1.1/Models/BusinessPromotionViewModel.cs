using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KYN_App_v1.Models
{
    //public class PromotionTimePeriod
    //{
    //    public int Day { get; set; }
    //    public Months Month { get; set; }
    //    public int Year { get; set; }
    //}

    public class ManagePromotionViewModel : KYN_App_v1._1.Models.BaseViewModel
    {
        public int BusinessDetailID { get; set; }
        public IList<PromotionDetailViewModel> PromotionDetailEntity { get; set; }
    }

    public class CreateBusinessPromotionViewModel : KYN_App_v1._1.Models.BaseViewModel
    {
        public IEnumerable<PromotionTypeViewModel> PromotionTypes { get; set; }
        public IEnumerable<PromotionValueTypeViewModel> PromotionValueTypes { get; set; }
        public IEnumerable<PromotionCouponValidForViewModel> PromotionCouponValidFor { get; set; }
        public PromotionDetailViewModel PromotionDetailViewModel { get; set; }
    }

    public class PromotionBasicViewModel
    {
        public IList<PromotionValueTypeViewModel> PromotionValueTypes { get; set; }
        public IList<PromotionTypeViewModel> PromotionTypes { get; set; }
        public IList<PromotionCouponValidForViewModel> PromotionCouponValidFor { get; set; }
    }

    public class PromotionCouponValidForViewModel
    {
        public int ID { get; set; }
        public string CouponValidFor { get; set; }
        public int ValidForInt { get; set; }
        public string Detail { get; set; }
    }

    public class PromotionValueTypeViewModel
    {
        public int ID { get; set; }
        public string ValueTypeName { get; set; }
        public string Description { get; set; }
    }

    public class PromotionTypeViewModel
    {
        public int ID { get; set; }
        public string PromotionTypeName { get; set; }
        public string PromotionTypeDescription { get; set; }
        public string PromotionTypeDetail { get; set; }
    }

    public class PromotionConditionViewModel
    {
        public long ID { get; set; }
        public string PromotionConditionDetail { get; set; }
    }


    public class PromotionResponseViewModel
    {
        public PromotionDetailViewModel PromotionDetailEntity { get; set; }
        public string PromotionStartDate { get; set; }
        public string PromotionEndDate { get; set; }
    }

    public class PromotionCreateUpdateViewModel
    {
        public PromotionDetailViewModel PromotionDetailEntity { get; set; }
        public string PromotionStartDate { get { return PromotionDetailEntity.PromotionStartDate.ToString(); } }
        public string PromotionEndDate { get { return PromotionDetailEntity.PromotionEndDate.ToString(); } }
    }

    public class GenerateCouponRequestEntity
    {
        public string UserID { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public int PromotionDetailID { get; set; }
        public int BusinessPrimaryDetailID { get; set; }
        //public string BusinessPrimaryDetailName { get; set; }
    }

    public class PromotionDetailViewModel
    {
        public int ID { get; set; }
        public string PromotionDetailName { get; set; }
        public string Guid { get; set; }
        public int RegionID { get; set; }
        public int PromotionTypeID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int DetailCategoryID { get; set; }
        public int BusinessPrimaryDetailID { get; set; }
        public string BusinessPrimaryDetailName { get; set; }
        public int PromotionConditionID { get; set; }
        [Display(Name = "Promotion Start Date")]
        [JsonIgnore]
        public DateTime PromotionStartDate { get; set; }
        [Display(Name = "Promotion End Date")]
        [JsonIgnore]
        public DateTime PromotionEndDate { get; set; }

        public string PromotionStartDateStr { get { return PromotionStartDate.ToString(); } }
        public string PromotionEndDateStr { get { return PromotionEndDate.ToString(); } }
        
        [Display(Name = "Total Quantity")]
        public int InitialQuantity { get; set; }
        public int StillLeftQuantity { get; set; }
        [Display(Name = "Promotion Value Type")]
        public int ValueTypeID { get; set; }
        public PromotionValueTypeViewModel ValueType { get; set; }
        public int CouponTotalValue { get; set; }
        [Display(Name = "Per Coupon Value")]
        public int PerCouponTotalValue { get; set; }
        [Display(Name = "Details")]
        public string PromotionDetails { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string PromotionTypeName { get; set; }
        public string PromotionTypeDescription { get; set; }
        public string PromotionConditionDetail { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }        
        [Display(Name = "Coupon Valid For")]
        public int CouponValidForID { get; set; }
        public PromotionCouponValidForViewModel CouponValidFor { get; set; }
        [Display(Name = "Per User Allowed Coupons")]
        public int PerUserAllowedCoupons { get; set; }
    }

    public class CouponVerifyViewModel : KYN_App_v1._1.Models.BaseViewModel
    {
        public IList<PromotionUserMappingResponseViewModel> PromotionCoupons { get; set; }
    }

    public class VerifyCouponRequestViewModel
    {
        public string UserMobileNumber { get; set; }
        public string UserEmail { get; set; }
        public int BusinessPrimaryDetailID { get; set; }
        public string BusinessUserID { get; set; }
        public string VerifyComment { get; set; }
        public string CouponCode { get; set; }

    }

    public class PromotionUserMappingResponseViewModel : KYN_App_v1._1.Models.BaseViewModel
    {
        public PromotionUserMappingViewModel PromotionUserMappingEntity { get; set; }
        public string CreateOn { get { return PromotionUserMappingEntity.CreateOn.ToString(); } }
        public string ValidTill { get { return PromotionUserMappingEntity.ValidTill.ToString(); } }
        public string VerifiedOn { get { return PromotionUserMappingEntity.VerifiedOn.ToString(); } }
        public string UsedOn { get { return PromotionUserMappingEntity.UsedOn.ToString(); } }
    }

    public class PromotionUserMappingViewModel
    {
        public int ID { get; set; }
        public string Guid { get; set; }
        public int RegionID { get; set; }
        public int PromotionDetailID { get; set; }
        public string PromotionDetailName { get; set; }
        public int BusinessDetailID { get; set; }
        public string BusinessDetailName { get; set; }
        public string UserID { get; set; }
        public bool IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public string VerifyComment { get; set; }
        public string CouponCode { get; set; }
        public string ValueType { get; set; }
        public int CouponValue { get; set; }
        public int PerCouponValue { get; set; }
        public bool IsSentToMobile { get; set; }
        public bool IsSentEmail { get; set; }

        public string Error { get; set; }

        public DateTime CreateOn { get { return System.Convert.ToDateTime(CreateOnStr); } }
        public DateTime ValidTill { get { return System.Convert.ToDateTime(ValidTillStr); } }
        public DateTime VerifiedOn { get { return System.Convert.ToDateTime(VerifiedOnStr); } }
        public DateTime UsedOn { get { return System.Convert.ToDateTime(UsedOnStr); } }

        public string CreateOnStr { get; set; }
        public string ValidTillStr { get; set; }
        public string VerifiedOnStr { get; set; }
        public string UsedOnStr { get; set; }
    }
}