using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [Hotfix]
    [DBTableName("item_level_selector_quality")]
    public sealed record ItemLevelSelectorQualityHotfix440: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("QualityItemBonusListID")]
        public int? QualityItemBonusListID;

        [DBFieldName("Quality")]
        public sbyte? Quality;

        [DBFieldName("ParentILSQualitySetID")]
        public int? ParentILSQualitySetID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
