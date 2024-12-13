using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V8_0_1_27101.Parsers
{
    public static class ChallengeModeHandler
    {
        public static void ReadModeAttemptChallengeModeMapStats(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            packet.ReadInt32("InstanceRealmAddress", indexes);
            packet.ReadInt32("AttemptID", indexes);
            packet.ReadTime("CompletionTime", indexes);
            packet.ReadTime("UnkTime4", indexes);
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_1_0_28724))
                packet.ReadTime("UnkTime5", indexes);
            packet.ResetBitReader();
            packet.ReadBit("UnkBit", indexes);
        }

        public static void ReadBMemberChallengeModeMapStats(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            packet.ReadPackedGuid128("PlayerGuid", indexes);
            packet.ReadPackedGuid128("GuildGuid", indexes);
            packet.ReadUInt32("VirtualRealmAddress", indexes);
            packet.ReadUInt32("NativeRealmAddress", indexes);
            packet.ReadInt16("SpecializationID", indexes);
            packet.ReadInt16("Unk4", indexes);
            packet.ReadInt32("EquipmentLevel", indexes);
        }

        public static void ReadChallengeModeMapStats(Packet packet, params object[] indexes)
        {
            packet.ResetBitReader();
            packet.ReadInt32("BestCompletionMilliseconds", indexes);
            packet.ReadUInt32("MapId", indexes);
            packet.ReadInt32("LastCompletionMilliseconds", indexes);
            packet.ReadTime("LastMedalDate", indexes);
            packet.ReadTime("BestMedalDate", indexes);

            for (int i = 0; i < 4; i++)
                packet.ReadUInt32("Affixes", indexes, i);

            var unkCount = packet.ReadUInt32("BMembersCount", indexes);
            for (int i = 0; i < unkCount; i++)
                ReadBMemberChallengeModeMapStats(packet, indexes, i);

            packet.ResetBitReader();
            packet.ReadBit("UnkBit", indexes);
        }

        [Parser(Opcode.CMSG_REQUEST_MYTHIC_PLUS_AFFIXES)]
        public static void HandleChallengeModeZero(Packet packet) { }

        [Parser(Opcode.SMSG_MYTHIC_PLUS_WEEKLY_REWARD_RESPONSE)]
        public static void HandleMythicPlusWeeklyRewardResponse(Packet packet)
        {
            packet.ReadBit("IsWeeklyRewardAvailable");

            packet.ReadInt32("LastWeekHighestKeyCompleted");
            packet.ReadInt32("LastWeekMapChallengeKeyEntry");
            packet.ReadInt32("CurrentWeekHighestKeyCompleted");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_1_0_28724))
                packet.ReadInt32("SeasonID"); // always 13 for me
        }

        [Parser(Opcode.SMSG_MYTHIC_PLUS_ALL_MAP_STATS)]
        public static void HandleMythicPlusAllMapStats(Packet packet)
        {
            var playerMapStatsCount = packet.ReadUInt32("PlayerMemberMapStats");
            var guildMemberMapStatsCount = packet.ReadUInt32("GuildMemberMapStats");
            var memberCount = packet.ReadUInt32("MemberCount");
            packet.ReadInt32("Season");
            if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_1_0_28724))
                packet.ReadInt32("SubSeason");

            for (int i = 0; i < playerMapStatsCount; i++)
                ReadBMemberChallengeModeMapStats(packet, i);

            for (int i = 0; i < guildMemberMapStatsCount; i++)
            {
                packet.ReadInt32("Unk7", i);
                if (ClientVersion.AddedInVersion(ClientVersionBuild.V8_1_0_28724))
                    packet.ReadInt32("Unk8", i);
                ReadBMemberChallengeModeMapStats(packet, i);
            }

            for (int i = 0; i < memberCount; i++)
                ReadModeAttemptChallengeModeMapStats(packet, i);
        }

        [Parser(Opcode.SMSG_MYTHIC_PLUS_CURRENT_AFFIXES)]
        public static void HandleMythicPlusCurrentAffixes(Packet packet)
        {
            var count = packet.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                packet.ReadInt32("KeystoneAffixID", i);
                packet.ReadInt32("RequiredSeason", i);
            }
        }
    }
}
