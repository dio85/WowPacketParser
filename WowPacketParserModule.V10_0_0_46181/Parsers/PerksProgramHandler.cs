using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V10_0_0_46181.Parsers
{
    public static class PerksProgramHandler
    {
        public static void ReadPerksVendorItem(Packet packet, params object[] idx)
        {
            packet.ReadInt32("VendorItemID", idx);
            packet.ReadInt32("MountID", idx);
            packet.ReadInt32("BattlePetSpeciesID", idx);
            packet.ReadInt32("TransmogSetID", idx);
            packet.ReadInt32("ItemModifiedAppearanceID",idx);
            packet.ReadInt32("TransmogIllusionID", idx);
            packet.ReadInt32("ToyID", idx);
            packet.ReadInt32("Price", idx);
            packet.ReadPackedTime("AvailableUntil", idx);
            packet.ReadBit("Disabled", idx);
        }
        [Parser(Opcode.SMSG_PERKS_PROGRAM_ACTIVITY_UPDATE)]
        public static void HandlePerksProgramActivityUpdate(Packet packet)
        {
            var activityCount = packet.ReadUInt32("ActivityCount");
            packet.ReadTime("TimeUntilEnd");
            packet.ReadInt32("MonthlyProgress");
            packet.ReadTime("TimeUntilStart");

            for (var i = 0; i < activityCount; i++)
                packet.ReadInt32("ActivityID", i);

            packet.ReadInt32("LastActivity");
            packet.ReadInt32("ActiveAvtivity");
        }

        [Parser(Opcode.SMSG_PERKS_PROGRAM_ACTIVITY_COMPLETE)]
        public static void HandlePerksProgramActivityComplete(Packet packet)
        {
            packet.ReadInt32("ActivityID");
        }

        [Parser(Opcode.SMSG_PERKS_PROGRAM_RESULT)]
        public static void HandlePerksProgramResult(Packet packet)
        {
            var type = packet.ReadBits("Type", 4);
            var hasUnkLong = packet.ReadBit("HasUnkLong");
            packet.ResetBitReader();
            if (hasUnkLong)
                packet.ReadUInt64("UnkLong");

            switch (type)
            {
                case 2: // BoughtItem
                    packet.ReadUInt32("VendorItemID");
                    var buyItemCount = packet.ReadUInt32("BuyItemCount");
                    for (var i = 0; i < buyItemCount; ++i)
                    {
                        packet.ReadUInt32("VendorItemID", i);
                        packet.ReadTime64("BuyTime", i);
                        packet.ReadByte("Flags");
                    }
                    break;
                case 4: // Collectors Cache
                    packet.ReadUInt32("UnkInt1");
                    packet.ReadUInt32("UnkInt2");
                    packet.ReadUInt32("RewardAmount"); // Monthly 500 Trader's Tender
                    var unkIntsCount = packet.ReadUInt32("UnkIntsCount");
                    for (var i = 0; i < unkIntsCount; ++i)
                        packet.ReadUInt32("UnkInt", i);
                    break;
                case 5: // AvailableItems
                    packet.ReadPackedGuid128("VendorGuid");
                    packet.ReadPackedGuid128("ModelSceneCameraGuid");
                    var itemCount = packet.ReadUInt32("VendorItemCount");
                    for (var i = 0; i < itemCount; ++i)
                        ReadPerksVendorItem(packet, i);
                    if (ClientVersion.AddedInVersion(ClientBranch.Retail, ClientVersionBuild.V12_0_0_65390))
                    {
                        packet.ReadByte("UnkByte1");
                        packet.ReadByte("UnkByte2");
                        packet.ReadInt32("CurrencyID");
                        packet.ReadInt16("UnkInt16");
                        packet.ReadInt32("UnkInt32");
                        ReadPerksVendorItem(packet, 999);
                        packet.ReadInt32("UnkInt32");
                        packet.ReadInt16("UnkInt16");
                        packet.ReadByte("UnkByte");
                        packet.ReadInt16("UnkInt16");
                    }
                    break;
                case 8:
                    packet.ReadInt32("UnkInt32");
                    break;
                case 10:
                    packet.ReadInt32("UnkInt32");
                    break;
                default:
                    break;
            }

        }

        [Parser(Opcode.CMSG_PERKS_PROGRAM_SET_FROZEN_VENDOR_ITEM)]
        public static void HandlerPerksProgramSetFrozenVendorItem(Packet packet)
        {
            packet.ReadBit("Set");
            packet.ReadUInt32("PerksVendorItemID");
            packet.ReadGuid("VendorGUID");
        }

        [Parser(Opcode.CMSG_PERKS_PROGRAM_REQUEST_PENDING_REWARDS)]
        [Parser(Opcode.CMSG_PERKS_PROGRAM_STATUS_REQUEST)]
        [Parser(Opcode.CMSG_UNKNOWN_PERK_PACKET)]
        public static void HandleNull(Packet packet) 
        {
        }
    }
}
