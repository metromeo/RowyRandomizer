using System;
using System.Collections.Generic;
using System.IO;

namespace RowyRandomizer
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			byte _weaponsCount = 55;
			HashSet<WeaponDelayIndex> _massiveWeapons = new HashSet<WeaponDelayIndex> { WeaponDelayIndex.EQuake, WeaponDelayIndex.Nuclear, WeaponDelayIndex.Arma };
			
			byte _massiveWeaponsMinDelay = args.Length > 0 ? byte.Parse(args[0]) : (byte)25;
			if (_massiveWeaponsMinDelay > _weaponsCount - _massiveWeapons.Count) _massiveWeaponsMinDelay = (byte)(_weaponsCount - _massiveWeapons.Count);
			
		    string root = Directory.GetCurrentDirectory();
		    string pathIn = Path.Combine(root, "RowyDefault.wsc");
		    string pathOut = Path.Combine(root, "RowyRandomized.wsc");
		    
		    BinaryReader reader = new BinaryReader(File.Open(pathIn, FileMode.Open, FileAccess.Read));
		    byte[] schemeData = new byte[reader.BaseStream.Length];
		    while (reader.BaseStream.Position != reader.BaseStream.Length)
			    schemeData[reader.BaseStream.Position] = reader.ReadByte();
		    reader.Close();
			
		    List<byte> indexPool_Massive = new List<byte>();
		    List<byte> indexPool_All = new List<byte>();

		    if (_massiveWeaponsMinDelay > 0)
			    FillPool(_massiveWeaponsMinDelay, _weaponsCount, ref indexPool_Massive);
		    FillPool(0, _massiveWeaponsMinDelay > 0 ? _massiveWeaponsMinDelay : _weaponsCount, ref indexPool_All);

		    Random randomizer = new Random();
			
		    //massive weapons first
		    if (_massiveWeaponsMinDelay > 0)
		    {
			    foreach (WeaponDelayIndex massiveWeapon in _massiveWeapons)
			    {
				    int random = randomizer.Next(0, indexPool_Massive.Count);
				    byte value = indexPool_Massive[random];
				    schemeData[(int)massiveWeapon] = value;
				    indexPool_Massive.Remove(value);
			    }
			    indexPool_All.AddRange(indexPool_Massive);
		    }
		    //rest of weapons
		    foreach (WeaponDelayIndex weaponDelayIndex in Enum.GetValues(typeof(WeaponDelayIndex)))
		    {
			    if (_massiveWeaponsMinDelay > 0 && _massiveWeapons.Contains(weaponDelayIndex)) continue;
			    int random = randomizer.Next(0, indexPool_All.Count);
			    byte value = indexPool_All[random];
			    schemeData[(int)weaponDelayIndex] = value;
			    indexPool_All.Remove(value);
		    }

		    BinaryWriter writer = new BinaryWriter(File.Open(pathOut, FileMode.OpenOrCreate, FileAccess.Write));
		    writer.Write(schemeData);
		    writer.Close();
		}
		
		private static void FillPool(byte min, byte max, ref List<byte> pool)
		{
			for (byte i = min; i < max; i++) pool.Add(i);
		}
		
		private enum WeaponDelayIndex
		{
			Bazooka = 43,
			Homing = 47,
			Mortar = 51,
			Grenade = 55,
			Cluster = 59,
			Skunk = 63,
			Petrol = 67,
			Banana = 71,
			Handgun = 75,
			Shotgun = 79,
			Uzi = 83,
			Minigun = 87,
			Longbow = 91,
			Airstrike = 95,
			Napalm = 99,
			Mine = 103,
			Firepunch = 107,
			Dragonball = 111,
			Kamikaze = 115,
			Prod = 119,
			Axe = 123,
			Blowtorch = 127,
			Drill = 131,
			Girder = 135,
			Rope = 139,
			Chute = 143,
			Bungee = 147,
			Tele = 151,
			Dynamite = 155,
			Sheep = 159,
			Bat = 163,
			Flamethrower = 167,
			Pigeon = 171,
			Cow = 175,
			Holy = 179,
			OldWoman = 183,
			SheepLauncher = 187,
			SuperSheep = 191,
			MoleBomb = 195,
			SuperBanana = 227,
			MineStrike = 231,
			GirderPack = 235,
			EQuake = 239,
			Scales = 244,
			MingVase = 247,
			CarpetBomb = 251,
			Nuclear = 259,
			Army = 267,
			Moles = 271,
			MB = 275,
			Donkey = 279,
			Suicide = 283,
			SheepStrike = 287,
			MailStrike = 291,
			Arma = 295,
		}
	}
}