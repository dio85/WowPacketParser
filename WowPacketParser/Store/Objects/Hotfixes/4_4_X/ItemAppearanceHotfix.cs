using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [Hotfix]
    [DBTableName("item_appearance")]
    public sealed record ItemAppearanceHotfix440: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("DisplayType")]
        public byte? DisplayType;

        [DBFieldName("ItemDisplayInfoID")]
        public int? ItemDisplayInfoID;

        [DBFieldName("DefaultIconFileDataID")]
        public int? DefaultIconFileDataID;

        [DBFieldName("UiOrder")]
        public int? UiOrder;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
    [Hotfix]
    [DBTableName("item_appearance")]
    public sealed record ItemAppearanceHotfix441: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("DisplayType")]
        public sbyte? DisplayType;

        [DBFieldName("ItemDisplayInfoID")]
        public int? ItemDisplayInfoID;

        [DBFieldName("DefaultIconFileDataID")]
        public int? DefaultIconFileDataID;

        [DBFieldName("UiOrder")]
        public int? UiOrder;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
