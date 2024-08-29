using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [Hotfix]
    [DBTableName("spell_aura_restrictions")]
    public sealed record SpellAuraRestrictionsHotfix440: IDataModel
    {
        [DBFieldName("ID", true)]
        public uint? ID;

        [DBFieldName("DifficultyID")]
        public byte? DifficultyID;

        [DBFieldName("CasterAuraState")]
        public byte? CasterAuraState;

        [DBFieldName("TargetAuraState")]
        public byte? TargetAuraState;

        [DBFieldName("ExcludeCasterAuraState")]
        public byte? ExcludeCasterAuraState;

        [DBFieldName("ExcludeTargetAuraState")]
        public byte? ExcludeTargetAuraState;

        [DBFieldName("CasterAuraSpell")]
        public int? CasterAuraSpell;

        [DBFieldName("TargetAuraSpell")]
        public int? TargetAuraSpell;

        [DBFieldName("ExcludeCasterAuraSpell")]
        public int? ExcludeCasterAuraSpell;

        [DBFieldName("ExcludeTargetAuraSpell")]
        public int? ExcludeTargetAuraSpell;

        [DBFieldName("SpellID")]
        public int? SpellID;

        [DBFieldName("VerifiedBuild")]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}
