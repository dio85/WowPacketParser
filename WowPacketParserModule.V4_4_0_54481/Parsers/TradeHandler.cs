﻿using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using TradeStatus = WowPacketParserModule.V4_4_0_54481.Enums.TradeStatus;

namespace WowPacketParserModule.V4_4_0_54481.Parsers
{
    public static class TradeHandler
    {
        [Parser(Opcode.SMSG_TRADE_STATUS)]
        public static void HandleTradeStatus(Packet packet)
        {
            packet.ReadBit("PartnerIsSameBnetAccount");
            var status = packet.ReadBitsE<TradeStatus>("Status", 5);

            switch (status)
            {
                case TradeStatus.Failed:
                    packet.ReadBit("FailureForYou");
                    packet.ReadInt32("BagResult");
                    packet.ReadInt32("ItemID");
                    break;
                case TradeStatus.Initiated:
                    packet.ReadUInt32("ID");
                    break;
                case TradeStatus.Proposed:
                    packet.ReadPackedGuid128("Partner");
                    packet.ReadPackedGuid128("PartnerAccount");
                    break;
                case TradeStatus.WrongRealm:
                case TradeStatus.NotOnTaplist:
                    packet.ReadByte("TradeSlot");
                    break;
                case TradeStatus.NotEnoughCurrency:
                case TradeStatus.CurrencyNotTradeable:
                    packet.ReadInt32("CurrencyType");
                    packet.ReadInt32("CurrencyQuantity");
                    break;
                default:
                    break;
            }
        }

        public static void ReadUnwrappedTradeItem(Packet packet, params object[] index)
        {
            packet.ReadInt32("EnchantID", index);
            packet.ReadInt32("OnUseEnchantmentID", index);
            packet.ReadPackedGuid128("Creator", index);
            packet.ReadInt32("Charges", index);
            packet.ReadInt32("MaxDurability", index);
            packet.ReadInt32("Durability", index);

            packet.ResetBitReader();
            var gemsCount = packet.ReadBits(2);
            packet.ReadBit("Lock", index);

            for (int j = 0; j < gemsCount; j++)
            {
                packet.ReadByte("Slot", index, j);
                Substructures.ItemHandler.ReadItemInstance(packet, index, j);
            }
        }

        public static void ReadTradeItem(Packet packet, params object[] index)
        {
            packet.ReadByte("Slot", index);
            packet.ReadInt32("StackCount", index);
            packet.ReadPackedGuid128("GiftCreator", index);
            Substructures.ItemHandler.ReadItemInstance(packet, index);

            packet.ResetBitReader();
            var unwrapped = packet.ReadBit("HasUnwrapped", index);
            if (unwrapped)
                ReadUnwrappedTradeItem(packet, index);
        }

        [Parser(Opcode.SMSG_TRADE_UPDATED)]
        public static void HandleTradeUpdated(Packet packet)
        {
            packet.ReadByte("WhichPlayer");

            packet.ReadInt32("ID");
            packet.ReadInt32("CurrentStateIndex");
            packet.ReadInt32("ClientStateIndex");

            packet.ReadInt64("Gold");

            // Order guessed
            packet.ReadInt32("CurrencyType");
            packet.ReadInt32("CurrencyQuantity");
            packet.ReadInt32("ProposedEnchantment");

            var count = packet.ReadInt32("ItemCount");

            for (int i = 0; i < count; i++)
                ReadTradeItem(packet, i);
        }

        [Parser(Opcode.CMSG_ACCEPT_TRADE)]
        public static void HandleAcceptTrade(Packet packet)
        {
            packet.ReadUInt32("StateIndex");
        }

        [Parser(Opcode.CMSG_CLEAR_TRADE_ITEM)]
        public static void HandleClearTradeItem(Packet packet)
        {
            packet.ReadByte("Slot");
        }

        [Parser(Opcode.CMSG_INITIATE_TRADE)]
        public static void HandleInitiateTrade(Packet packet)
        {
            packet.ReadPackedGuid128("GUID");
        }

        [Parser(Opcode.CMSG_SET_TRADE_GOLD)]
        public static void HandleTradeGold(Packet packet)
        {
            packet.ReadUInt64("Coinage");
        }

        [Parser(Opcode.CMSG_SET_TRADE_ITEM)]
        public static void HandleTradeItem(Packet packet)
        {
            packet.ReadByte("TradeSlot");
            packet.ReadByte("PackSlot");
            packet.ReadByte("ItemSlotInPack");
        }

        [Parser(Opcode.CMSG_BEGIN_TRADE)]
        [Parser(Opcode.CMSG_BUSY_TRADE)]
        [Parser(Opcode.CMSG_CANCEL_TRADE)]
        [Parser(Opcode.CMSG_IGNORE_TRADE)]
        [Parser(Opcode.CMSG_UNACCEPT_TRADE)]
        public static void HandleTradeNull(Packet packet)
        {
        }
    }
}
