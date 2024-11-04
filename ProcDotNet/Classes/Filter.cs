namespace ProcDotNet.Classes
{
    internal class Architecture
    {
        public const string x86 = "32-bit";
        public const string x64 = "64-bit";
    }

    public class EventClass
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

    internal class Operation
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
        public const string QueryNormalizedNameInformationFile = "QueryNormalizedNameInformationFile";
        public const string QueryNumaNodeInformation = "QueryNumaNodeInformation";
        public const string QueryObjectIDInformationVolume = "QueryObjectIDInformationVolume";
        public const string QueryOpen = "QueryOpen";
        public const string QueryPNPDeviceState = "QueryPNPDeviceState";
        public const string QueryPositionInformationFile = "QueryPositionInformationFile";
        public const string QueryRemoteProtocolInformation = "QueryRemoteProtocolInformation";
        public const string QueryRemoveDevice = "QueryRemoveDevice";
        public const string QueryRenameInformationBypassAccessCheck = "QueryRenameInformationBypassAccessCheck";
        public const string QueryResourceRequirements = "QueryResourceRequirements";
        public const string QueryResources = "QueryResources";
        public const string QuerySatLXInformation = "QuerySatLXInformation";
        public const string QuerySecurityFile = "QuerySecurityFile";
        public const string QueryShortNameInformationFile = "QueryShortNameInformationFile";
        public const string QuerySizeInformtionVolume = "QuerySizeInformtionVolume";
        public const string QueryStandardInformationFile = "QueryStandardInformationFile";
        public const string QueryStandardLinkInformation = "QueryStandardLinkInformation";
        public const string QueryStatInformation = "QueryStatInformation";
        public const string QueryStopDevice = "QueryStopDevice";
        public const string QueryStorageReservedIDInformation = "QueryStorageReservedIDInformation";
        public const string QueryStreamInformationFile = "QueryStreamInformationFile";
        public const string QueryValidDataLength = "QueryValidDataLength";
        public const string QueryVolumeNameInformation = "QueryVolumeNameInformation";
        public const string ReadConfig = "ReadConfig";
        public const string ReadFile = "ReadFile";
        public const string RegCloseKey = "RegCloseKey";
        public const string RegCreateKey = "RegCreateKey";
        public const string RegDeleteKey = "RegDeleteKey";
        public const string RegDeleteValue = "RegDeleteValue";
        public const string RegEnumKey = "RegEnumKey";
        public const string RegEnumValue = "RegEnumValue";
        public const string RegFlushKey = "RegFlushKey";
        public const string RegLoadKey = "RegLoadKey";
        public const string RegOpenKey = "RegOpenKey";
        public const string RegOpenKey2 = "RegOpenKey2";
        public const string RegQueryKey = "RegQueryKey";
        public const string RegQueryKeySecurity = "RegQueryKeySecurity";
        public const string RegQueryMultipleValueKey = "RegQueryMultipleValueKey";
        public const string RegQueryValue = "RegQueryValue";
        public const string RegRenameKey = "RegRenameKey";
        public const string RegReplaceKey = "RegReplaceKey";
        public const string RegRestoreKey = "RegRestoreKey";
        public const string RegSaveKey = "RegSaveKey";
        public const string RegSetInfoKey = "RegSetInfoKey";
        public const string RegSetKeySecurity = "RegSetKeySecurity";
        public const string RegSetValue = "RegSetValue";
        public const string RegUnloadKey = "RegUnloadKey";
        public const string RemoveDevice = "RemoveDevice";
        public const string SetBasicInformationFile = "SetBasicInformationFile";
        public const string SetDispositionInformationEx = "SetDispositionInformationEx";
        public const string SetDispositionInformationFile = "SetDispositionInformationFile";
        public const string SetEAFile = "SetEAFile";
        public const string SetEndOfFileInformationFile = "SetEndOfFileInformationFile";
        public const string SetFileQuota = "SetFileQuota";
        public const string SetFileStreamInformation = "SetFileStreamInformation";
        public const string SetLinkInformationFile = "SetLinkInformationFile";
        public const string SetLock = "SetLock";
        public const string SetPipeInformation = "SetPipeInformation";
        public const string SetPositionInformationFile = "SetPositionInformationFile";
        public const string SetRenameInformationEx = "SetRenameInformationEx";
        public const string SetRenameInformationExBypassAccessCheck = "SetRenameInformationExBypassAccessCheck";
        public const string SetRenameInformationFile = "SetRenameInformationFile";
        public const string SetReplaceCompletionInformation = "SetReplaceCompletionInformation";
        public const string SetSecurityFile = "SetSecurityFile";
        public const string SetShortNameInformation = "SetShortNameInformation";
        public const string SetStorageReservedIDInformation = "SetStorageReservedIDInformation";
        public const string SetValidDataLengthInformationFile = "SetValidDataLengthInformationFile";
        public const string SetVolumeInformation = "SetVolumeInformation";
        public const string Shutdown = "Shutdown";
        public const string StartDevice = "StartDevice";
        public const string StopDevice = "StopDevice";
        public const string SurpriseRemoval = "SurpriseRemoval";
        public const string SystemStatistics = "SystemStatistics";
        public const string SystemControl = "SystemControl";
        public const string TCPAccept = "TCP Accept";
        public const string TCPConnect = "TCP Connect";
        public const string TCPDisconnect = "TCP Disconnect";
        public const string TCPOther = "TCP Other";
        public const string TCPReceive = "TCP Receive";
        public const string TCPReconnect = "TCP Reconnect";
        public const string TCPRetransmit = "TCP Retransmit";
        public const string TCPSend = "TCP Send";
        public const string TCPTCPCopy = "TCP TCPCopy";
        public const string TCPUnknown = "TCP Unknown";
        public const string ThreadCreate = "ThreadCreate";
        public const string ThreadExit = "ThreadExit";
        public const string ThreadProfile = "ThreadProfile";
        public const string ThreadProfiling = "ThreadProfiling";
        public const string UDPAccept = "UDP Accept";
        public const string UDPConnect = "UDP Connect";
        public const string UDPDisconnect = "UDP Disconnect";
        public const string UDPOther = "UDP Other";
        public const string UDPRecieve = "UDP Recieve";
        public const string UDPReconnect = "UDP Reconnect";
        public const string UDPRetransmit = "UDP Retransmit";
        public const string UDPSend = "UDP Send";
        public const string UDPTCPCOPY = "UDP TCPCOPY";
        public const string UDPUnknown = "UDP Unknown";
        public const string UnlockFileAll = "UnlockFileAll";
        public const string UnlockFileByKey = "UnlockFileByKey";
        public const string UnlockFileSingle = "UnlockFileSingle";
        public const string VolumeDismount = "VolumeDismount";
        public const string VolumeMount = "VolumeMount";
        public const string WriteConfig = "WriteConfig";
        public const string WriteFile = "WriteFile";


    }

}
