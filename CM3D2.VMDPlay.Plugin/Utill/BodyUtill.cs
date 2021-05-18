using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CM3D2.VMDPlay.Plugin.Utill
{
    class BodyUtill
    {
        public readonly static Dictionary<string, string> boneNameMap = new Dictionary<string, string>
			{
				{
					"Bip01",
					"センター"
				},
				{
					"Bip01 Spine",
					"上半身"
				},
				{
					"Bip01 Spine1",
					"上半身2"
				},
				{
					"Bip01 Spine1a",
					"上半身2先"
				},
				{
					"Bip01 Neck",
					"首"
				},
				{
					"Bip01 Head",
					"頭"
				},
				{
					"Bip01 L Clavicle",
					"左肩"
				},
				{
					"Bip01 L UpperArm",
					"左腕"
				},
				{
					"Bip01 L Forearm",
					"左ひじ"
				},
				{
					"Bip01 L Hand",
					"左手首"
				},
				{
					"Bip01 R Clavicle",
					"右肩"
				},
				{
					"Bip01 R UpperArm",
					"右腕"
				},
				{
					"Bip01 R Forearm",
					"右ひじ"
				},
				{
					"Bip01 R Hand",
					"右手首"
				},
				{
					"Bip01 Pelvis",
					"下半身"
				},
				{
					"Bip01 L Thigh",
					"左足"
				},
				{
					"Bip01 L Calf",
					"左ひざ"
				},
				{
					"Bip01 L Foot",
					"左足首"
				},
				{
					"Bip01 R Thigh",
					"右足"
				},
				{
					"Bip01 R Calf",
					"右ひざ"
				},
				{
					"Bip01 R Foot",
					"右足首"
				},
				{
					"Bip01 L Finger01",
					"左親指１"
				},
				{
					"Bip01 L Finger02",
					"左親指２"
				},
				{
					"Bip01 L Finger1",
					"左人指１"
				},
				{
					"Bip01 L Finger11",
					"左人指２"
				},
				{
					"Bip01 L Finger12",
					"左人指３"
				},
				{
					"Bip01 L Finger2",
					"左中指１"
				},
				{
					"Bip01 L Finger21",
					"左中指２"
				},
				{
					"Bip01 L Finger22",
					"左中指３"
				},
				{
					"Bip01 L Finger3",
					"左薬指１"
				},
				{
					"Bip01 L Finger31",
					"左薬指２"
				},
				{
					"Bip01 L Finger32",
					"左薬指３"
				},
				{
					"Bip01 L Finger4",
					"左小指１"
				},
				{
					"Bip01 L Finger41",
					"左小指２"
				},
				{
					"Bip01 L Finger42",
					"左小指３"
				},
				{
					"Bip01 R Finger01",
					"右親指１"
				},
				{
					"Bip01 R Finger02",
					"右親指２"
				},
				{
					"Bip01 R Finger1",
					"右人指１"
				},
				{
					"Bip01 R Finger11",
					"右人指２"
				},
				{
					"Bip01 R Finger12",
					"右人指３"
				},
				{
					"Bip01 R Finger2",
					"右中指１"
				},
				{
					"Bip01 R Finger21",
					"右中指２"
				},
				{
					"Bip01 R Finger22",
					"右中指３"
				},
				{
					"Bip01 R Finger3",
					"右薬指１"
				},
				{
					"Bip01 R Finger31",
					"右薬指２"
				},
				{
					"Bip01 R Finger32",
					"右薬指３"
				},
				{
					"Bip01 R Finger4",
					"右小指１"
				},
				{
					"Bip01 R Finger41",
					"右小指２"
				},
				{
					"Bip01 R Finger42",
					"右小指３"
				},
				{
					"_FOOT_IK_L",
					"左足ＩＫ"
				},
				{
					"_FOOT_IK_R",
					"右足ＩＫ"
				},
				{
					"_TOE_IK_L",
					"左つま先ＩＫ"
				},
				{
					"_TOE_IK_R",
					"右つま先ＩＫ"
				}
			};

		public readonly static List<string> boneNames = new List<string>
		{
			"Bip01",
			"Bip01/Bip01 Spine",
			"Bip01/Bip01 Spine/Bip01 Spine0a",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 Neck",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 Neck/Bip01 Head",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger0",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger01",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger02",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger11",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger12",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger2",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger21",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger22",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger3",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger31",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger32",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger4",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger41",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger42",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger0",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger01",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger02",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger11",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger12",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger2",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger21",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger22",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger3",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger31",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger32",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger4",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger41",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger42",
			"Bip01/Bip01 Pelvis",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0/Bip01 L Toe01",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0/Bip01 L Toe01/Bip01 L Toe0Nub",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1/Bip01 L Toe11",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1/Bip01 L Toe11/Bip01 L Toe1Nub",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2/Bip01 L Toe21",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2/Bip01 L Toe21/Bip01 L Toe2Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0/Bip01 R Toe01",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0/Bip01 R Toe01/Bip01 R Toe0Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1/Bip01 R Toe11",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1/Bip01 R Toe11/Bip01 R Toe1Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2/Bip01 R Toe21",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2/Bip01 R Toe21/Bip01 R Toe2Nub",
			"_FOOT_IK_L",
			"_FOOT_IK_R",
			"_FOOT_IK_L/_TOE_IK_L",
			"_FOOT_IK_R/_TOE_IK_R"
		};
	}
}
