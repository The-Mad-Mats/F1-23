using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace F1
{
    public static class UDPConverter
    {
        public static PacketHeader GetPacketHeader(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketHeader)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketHeader));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketMotionData ConvertToMotionData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketMotionData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketMotionData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketSessionData ConvertToSessionData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketSessionData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketSessionData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketParticipantsData ConvertToParticipantData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketParticipantsData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketParticipantsData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketLapData ConvertToLapData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketLapData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketLapData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketEventData ConvertToEventData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketEventData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketEventData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketCarSetupData ConvertToCarSetupData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketCarSetupData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketCarSetupData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketCarTelemetryData ConvertToCarTelemetryData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketCarTelemetryData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketCarTelemetryData));
            pinnedPacket.Free();
            return msg;
        }

        public static PacketCarStatusData ConvertToCarStatusData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketCarStatusData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketCarStatusData));
            pinnedPacket.Free();
            return msg;
        }
        public static PacketCarDamageData ConvertToCarDamageData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketCarDamageData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketCarDamageData));
            pinnedPacket.Free();
            return msg;
        }
        public static PacketSessionHistoryData ConvertToSessionHistoryData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketSessionHistoryData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketSessionHistoryData));
            pinnedPacket.Free();
            return msg;
        }
        public static PacketFinalClassificationData ConvertToFinalClassificationData(byte[] packet)
        {
            var pinnedPacket = GCHandle.Alloc(packet, GCHandleType.Pinned);
            var msg = (PacketFinalClassificationData)Marshal.PtrToStructure(
                pinnedPacket.AddrOfPinnedObject(),
                typeof(PacketFinalClassificationData));
            pinnedPacket.Free();
            return msg;
        }
    }
}
