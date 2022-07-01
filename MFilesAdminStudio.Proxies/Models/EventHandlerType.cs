using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies.Models
{
	// generated from:
	// EnumToEnumGenerator.Generate<MFEventHandlerType>(
	//    generatedEnumName: "EventHandlerType",
	//    fieldNameGenerator: e => e.ToString().Substring("MFEventHandler".Length),
	//    fieldDescriptionGenerator: e => e.ToString().Substring("MFEventHandler".Length).Humanize(LetterCasing.Title)); 
	public enum EventHandlerType
	{
		[Description("Type Undefined")]
		TypeUndefined = 0,

		[Description("Before Set Properties")]
		BeforeSetProperties = 1,

		[Description("After Set Properties")]
		AfterSetProperties = 2,

		[Description("After Create New Object Finalize")]
		AfterCreateNewObjectFinalize = 3,

		[Description("Before Check In Changes")]
		BeforeCheckInChanges = 4,

		[Description("After Check In Changes")]
		AfterCheckInChanges = 5,

		[Description("Before Check Out")]
		BeforeCheckOut = 6,

		[Description("After Check Out")]
		AfterCheckOut = 7,

		[Description("Before Cancel Checkout")]
		BeforeCancelCheckout = 8,

		[Description("After Cancel Checkout")]
		AfterCancelCheckout = 9,

		[Description("Before Delete Object")]
		BeforeDeleteObject = 10,

		[Description("After Delete Object")]
		AfterDeleteObject = 11,

		[Description("Before Destroy Object")]
		BeforeDestroyObject = 12,

		[Description("After Destroy Object")]
		AfterDestroyObject = 13,

		[Description("Before Set Object Permissions")]
		BeforeSetObjectPermissions = 14,

		[Description("After Set Object Permissions")]
		AfterSetObjectPermissions = 15,

		[Description("Before File Upload")]
		BeforeFileUpload = 16,

		[Description("After File Upload")]
		AfterFileUpload = 17,

		[Description("Before File Download")]
		BeforeFileDownload = 18,

		[Description("After File Download")]
		AfterFileDownload = 19,

		[Description("Before Create New Value List Item")]
		BeforeCreateNewValueListItem = 20,

		[Description("After Create New Value List Item")]
		AfterCreateNewValueListItem = 21,

		[Description("Before Login To Vault")]
		BeforeLoginToVault = 22,

		[Description("After Login To Vault")]
		AfterLoginToVault = 23,

		[Description("Before Logout From Vault")]
		BeforeLogoutFromVault = 24,

		[Description("After Logout From Vault")]
		AfterLogoutFromVault = 25,

		[Description("Before Run Scheduled Job")]
		BeforeRunScheduledJob = 26,

		[Description("After Run Scheduled Job")]
		AfterRunScheduledJob = 27,

		[Description("Before Create New Object Finalize")]
		BeforeCreateNewObjectFinalize = 28,

		[Description("Before Cancel Create Object")]
		BeforeCancelCreateObject = 29,

		[Description("After Cancel Create Object")]
		AfterCancelCreateObject = 30,

		[Description("Before Destroy Object Version")]
		BeforeDestroyObjectVersion = 31,

		[Description("After Destroy Object Version")]
		AfterDestroyObjectVersion = 32,

		[Description("Replication Aftercreatenewobjectfinalize")]
		Replication_AfterCreateNewObjectFinalize = 33,

		[Description("Replication Aftercheckınchanges")]
		Replication_AfterCheckInChanges = 34,

		[Description("Vault Extension Method")]
		VaultExtensionMethod = 35,

		[Description("Before Create Login Account")]
		BeforeCreateLoginAccount = 36,

		[Description("After Create Login Account")]
		AfterCreateLoginAccount = 37,

		[Description("Before Modify Login Account")]
		BeforeModifyLoginAccount = 38,

		[Description("After Modify Login Account")]
		AfterModifyLoginAccount = 39,

		[Description("Before Remove Login Account")]
		BeforeRemoveLoginAccount = 40,

		[Description("After Remove Login Account")]
		AfterRemoveLoginAccount = 41,

		[Description("Before Create User Account")]
		BeforeCreateUserAccount = 42,

		[Description("After Create User Account")]
		AfterCreateUserAccount = 43,

		[Description("Before Modify User Account")]
		BeforeModifyUserAccount = 44,

		[Description("After Modify User Account")]
		AfterModifyUserAccount = 45,

		[Description("Before Remove User Account")]
		BeforeRemoveUserAccount = 46,

		[Description("After Remove User Account")]
		AfterRemoveUserAccount = 47,

		[Description("Before Create User Group")]
		BeforeCreateUserGroup = 48,

		[Description("After Create User Group")]
		AfterCreateUserGroup = 49,

		[Description("Before Modify User Group")]
		BeforeModifyUserGroup = 50,

		[Description("After Modify User Group")]
		AfterModifyUserGroup = 51,

		[Description("Before Remove User Group")]
		BeforeRemoveUserGroup = 52,

		[Description("After Remove User Group")]
		AfterRemoveUserGroup = 53,

		[Description("After Bring Online")]
		AfterBringOnline = 54,

		[Description("Before Take Offline")]
		BeforeTakeOffline = 55,

		[Description("After Check In Changes Finalize")]
		AfterCheckInChangesFinalize = 56,

		[Description("After Begin Transaction")]
		AfterBeginTransaction = 57,

		[Description("Before Commit Transaction")]
		BeforeCommitTransaction = 58,

		[Description("Before Rollback Transaction")]
		BeforeRollbackTransaction = 59,

		[Description("After Cancel Checkout Finalize")]
		AfterCancelCheckoutFinalize = 60,

		[Description("Before Undelete Object")]
		BeforeUndeleteObject = 61,

		[Description("After Undelete Object")]
		AfterUndeleteObject = 62,

		[Description("After Undelete Object Finalize")]
		AfterUndeleteObjectFinalize = 63,

		[Description("Before Modify M Files Credentials")]
		BeforeModifyMFilesCredentials = 64,

		[Description("After Modify M Files Credentials")]
		AfterModifyMFilesCredentials = 65,

		[Description("Before Return View")]
		BeforeReturnView = 66,

		[Description("Before Check In Changes Finalize")]
		BeforeCheckInChangesFinalize = 67,

		[Description("Before Create View")]
		BeforeCreateView = 68,

		[Description("After Create View")]
		AfterCreateView = 69,

		[Description("Before Modify View")]
		BeforeModifyView = 70,

		[Description("After Modify View")]
		AfterModifyView = 71,

		[Description("Before Delete View")]
		BeforeDeleteView = 72,

		[Description("After Delete View")]
		AfterDeleteView = 73,

		[Description("Before Add User Group Member")]
		BeforeAddUserGroupMember = 74,

		[Description("After Add User Group Member")]
		AfterAddUserGroupMember = 75,

		[Description("Before Remove User Group Member")]
		BeforeRemoveUserGroupMember = 76,

		[Description("After Remove User Group Member")]
		AfterRemoveUserGroupMember = 77,
	}
}
