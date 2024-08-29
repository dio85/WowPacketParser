using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [Hotfix]
    [DBTableName("item")]
    public sealed record ItemHotfix440: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("ClassID")]
        public byte? ClassID;

        [DBFieldName("SubclassID")]
        public byte? SubclassID;

        [DBFieldName("Material")]
        public byte? Material;

        [DBFieldName("InventoryType")]
        public sbyte? InventoryType;

        [DBFieldName("RequiredLevel")]
        public int? RequiredLevel;

        [DBFieldName("SheatheType")]
        public byte? SheatheType;

        [DBFieldName("RandomSelect")]
        public ushort? RandomSelect;

        [DBFieldName("ItemRandomSuffixGroupID")]
        public ushort? ItemRandomSuffixGroupID;

        [DBFieldName("SoundOverrideSubclassID")]
        public sbyte? SoundOverrideSubclassID;

        [DBFieldName("ScalingStatDistributionID")]
        public ushort? ScalingStatDistributionID;

        [DBFieldName("IconFileDataID")]
        public int? IconFileDataID;

        [DBFieldName("ItemGroupSoundsID")]
        public byte? ItemGroupSoundsID;

        [DBFieldName("ContentTuningID")]
        public int? ContentTuningID;

        [DBFieldName("MaxDurability")]
        public uint? MaxDurability;

        [DBFieldName("AmmunitionType")]
        public byte? AmmunitionType;

        [DBFieldName("ScalingStatValue")]
        public int? ScalingStatValue;

        [DBFieldName("DamageType", 5)]
        public byte?[] DamageType;

        [DBFieldName("Resistances", 7)]
        public int?[] Resistances;

        [DBFieldName("MinDamage", 5)]
        public int?[] MinDamage;

        [DBFieldName("MaxDamage", 5)]
        public int?[] MaxDamage;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
