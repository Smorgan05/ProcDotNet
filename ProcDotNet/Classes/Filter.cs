using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet.Classes
{
    internal class Filter
    {
        internal class Architecture
        {
            public const string x86 = "32-bit";
            public const string x64 = "64-bit";
        }

        internal class EventClass
        {
            public const string FileSystem = "File System";
            public const string Network = "Network";
            public const string Process = "Process";
            public const string Profiling = "Profiling";
            public const string Registry = "Registry";
        }

        internal class Result
        {
            public const string Success = "Success";
            public const string Unknown = "Unknown";
        }

        internal class Virtualized
        {
            public const string True = "true";
            public const string False = "false";
            public const string na = "n/a";
        }

        internal class Operation // Incomplete
        {
            public const string CancelRemoveDevice = "CancelRemoveDevice";
            public const string CancelStopDevice = "CancelStopDevice";
            public const string CloseFile = "CloseFile";
            public const string CreateFile = "CreateFile";
            public const string CreateFileMapping = "CreateFileMapping";

            public const string CreateMailSlot = "CreateMailSlot";
            public const string CreatePipe = "CreatePipe";
            public const string DebugOutputProfiling = "DebugOutputProfiling";
            public const string DeviceChange = "DeviceChange";
            public const string DeviceIOControl = "DeviceIOControl";
            public const string DeviceUsuageNotification = "DeviceUsuageNotification";
            public const string Eject = "Eject";
            public const string FileSystemControl = "FileSystemControl";
            public const string FilterResourceRequirements = "FilterResourceRequirements";
            public const string FlushBuffersFile = "FlushBuffersFile";
            public const string InternalDeviceIOControl = "InternalDeviceIOControl";
            public const string LoadImage = "LoadImage";
            public const string LockFile = "LockFile";
            public const string NotifyChangeDirectory = "NotifyChangeDirectory";
            public const string Power = "Power";
            public const string ProcessCreate = "ProcessCreate";
            public const string ProcessExit = "ProcessExit";
            public const string ProcessProfiling = "ProcessProfiling";
            public const string ProcessStart = "ProcessStart";
            public const string ProcessStatistics = "ProcessStatistics";
            public const string QueryAllInformaitonFile = "QueryAllInformaitonFile";
            public const string QueryAttributeCacheInformation = "QueryAttributeCacheInformation";
            public const string QueryAttributeInformationVolume = "QueryAttributeInformationVolume";
            public const string QueryAttributeTag = "QueryAttributeTag";
            public const string QueryAttributeTagFile = "QueryAttributeTagFile";
            public const string QueryBasicInformationFile = "QueryBasicInformationFile";
            public const string QueryBusInformation = "QueryBusInformation";
            public const string QueryCapabilities = "QueryCapabilities";
            public const string QueryCaseSensitiveInformation = "QueryCaseSensitiveInformation";
            public const string QueryCaseSensitiveInformationForceAccessCheck = "QueryCaseSensitiveInformationForceAccessCheck";
            public const string QueryCompressInformationFile = "QueryCompressInformationFile";
            public const string QueryControlInformationVolume = "QueryControlInformationVolume";
            public const string QueryDesiredStorageClassInformation = "QueryDesiredStorageClassInformation";
            public const string QueryDevinceInformationVolume = "QueryDevinceInformationVolume";
            public const string QueryDeviceRelations = "QueryDeviceRelations";
            public const string QueryDeviceText = "QueryDeviceText";
            public const string QueryDirectory = "QueryDirectory";
            public const string QueryEAFile = "QueryEAFile";
            public const string QueryEAInformationFile = "QueryEAInformationFile";
            public const string QueryEndofFile = "QueryEndofFile";
            public const string QueryFileInternalInformationFile = "QueryFileInternalInformationFile";
            public const string QueryFileQuota = "QueryFileQuota";
            public const string QueryFullSizeInformationVolume = "QueryFullSizeInformationVolume";
            public const string QueryHardLinkFullInformation = "QueryHardLinkFullInformation";
            public const string QueryID = "QueryID";
            public const string QueryIDBothDirectory = "QueryIDBothDirectory";
            public const string QueryIDExtdBothDirectoryInformation = "QueryIDExtdBothDirectoryInformation";
            public const string QueryIDExtdDirectoryInformation = "QueryIDExtdDirectoryInformation";
            public const string QueryIDGlobalTXDirectoryInformation = "QueryIDGlobalTXDirectoryInformation";
            public const string QueryIDInformation = "QueryIDInformation";
            public const string QueryInformationVolume = "QueryInformationVolume";
            public const string QueryInterface = "QueryInterface";
            public const string QueryIOPriorityHint = "QueryIOPriorityHint";
            public const string QueryIsRemoveDeviceInformation = "QueryIsRemoveDeviceInformation";
            public const string QueryLabelInformationVolume = "QueryLabelInformationVolume";
            public const string QueryLegacyBusInformation = "QueryLegacyBusInformation";
            public const string QueryLinkInformationBypassAccessCheck = "QueryLinkInformationBypassAccessCheck";
            public const string QueryLinkInformationEx = "QueryLinkInformationEx";
            public const string QueryLinks = "QueryLinks";

            public const string QueryMemoryPartitionInformation = "QueryMemoryPartitionInformation";
            public const string QueryMoveClusterInformationFile = "QueryMoveClusterInformationFile";
            public const string QueryNameInformationFile = "QueryNameInformationFile";
            public const string QueryNetworkOpenInformationFile = "QueryNetworkOpenInformationFile";
            public const string QueryNetworkPhysicalNameInformationFile = "QueryNetworkPhysicalNameInformationFile";
            public const string QueryNormalizedNameInformationFile = "";
            public const string QueryNumaNodeInformation = "";
            public const string QueryObjectIDInformationVolume = "";
            public const string QueryOpen = "";
            public const string QueryPNPDeviceState = "";
            public const string QueryPositionInformationFile = "";
            public const string QueryRemoteProtocolInformation = "";
            public const string QueryRemoveDevice = "";
            public const string QueryRenameInformationBypassAccessCheck = "";
            public const string QueryResourceRequirements = "";
            public const string QueryResources = "";
            public const string QuerySatLXInformation = "";
            public const string QuerySecurityFile = "";
            public const string QueryShortNameInformationFile = "";
            public const string QuerySizeInformtionVolume = "";
            public const string QueryStandardInformationFile = "";
            public const string QueryStandardLinkInformation = "";
            public const string QueryStatInformation = "";
            public const string QueryStopDevice = "";
            public const string QueryStorageReservedIDInformation = "";
            public const string QueryStreamInformationFile = "";
            public const string QueryValidDataLength = "";
            public const string QueryVolumeNameInformation = "";
            public const string ReadConfig = "";
            public const string ReadFile = "";


           

        }





    }
}
