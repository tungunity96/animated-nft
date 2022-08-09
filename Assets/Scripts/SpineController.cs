using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;

public class SpineController : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState animationState;
    Spine.Skeleton skeleton;
    Spine.SkeletonData skeletonData;
    SkeletonAnimation skinSkeletonAnimation;

    [Header("Character Atlases")]
    public SpineAtlasAsset rootAtlas;
    public SpineAtlasAsset weaponAtlas;
    public SpineAtlasAsset classAtlas;
    public SpineAtlasAsset raceAtlas;
    public SpineAtlasAsset classForeHandAtlas;
    public SpineAtlasAsset hairAtlas;
    public SpineAtlasAsset eyesAtlas;

    [Header("Change character trails")]
    public bool isFemale = false;
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.Skeleton;
        skeletonData = skeleton.Data;
        animationState = skeletonAnimation.AnimationState;
    }

    public IEnumerator PlayAnimation(string animName, bool loop = false)
    {
        Debug.Log("Play Animation " + animName);
        var track = animationState.SetAnimation(0, animName, false);
        yield return new WaitForSpineAnimationComplete(track, true);
        if (loop) yield return PlayAnimation(animName, loop);
    }

    public void ChangeSkin(RaceType raceType, ClassType classType, string eyeName, string hairName)
    {
        ChangeRaceRegion(raceType);
        ChangeClassRegion(classType);
        ChangeClassForeHandRegion(classType);
        ChangeEyesRegion(eyeName);
        ChangeHairRegion(hairName);
        ChangeWeaponRegion((WeaponType)classType);
        UpdateSkeletonAnimation();
    }

    public void ChangeWeaponRegion(WeaponType weaponType)
    {
        SetSlotRegion(AtlasType.Weapon, "Weapon", "Weapon/" + weaponType.ToString());
    }
    public void ChangeClassForeHandRegion(ClassType classType)
    {
        SetSlotRegion(AtlasType.ClassForeHand, "Class_fore_hand", "Class_fore_hand/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_fore_hand");
    }

    public void ChangeHairRegion(string hairName)
    {
        SetSlotRegion(AtlasType.Hair, "B_Hair", "B_Hair/" + (isFemale ? "FM_" : "M_") + "B_Hair_" + hairName);
        SetSlotRegion(AtlasType.Hair, "F_Hair", "F_Hair/" + (isFemale ? "FM_" : "M_") + "F_Hair_" + hairName);
    }
    public void ChangeEyesRegion(string eyeName)
    {
        SetSlotRegion(AtlasType.Eyes, "Eyes", "Eyes/" + (isFemale ? "FM_" : "M_") + "Eyes_" + eyeName);
    }

    public void ChangeClassRegion(ClassType classType)
    {
        SetSlotRegion(AtlasType.Class, "Class_Body", "Class_Body/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_Body");
        SetSlotRegion(AtlasType.Class, "Class_back_arm1", "Class_back_arm1/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_arm1");
        SetSlotRegion(AtlasType.Class, "Class_back_arm2", "Class_back_arm2/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_arm2");
        SetSlotRegion(AtlasType.Class, "Class_fore_arm1", "Class_fore_arm1/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_fore_arm1");
        SetSlotRegion(AtlasType.Class, "Class_fore_arm2", "Class_fore_arm2/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_fore_arm2");
        SetSlotRegion(AtlasType.Class, "Class_fore_coat", "Class_fore_coat/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_fore_coat");
        SetSlotRegion(AtlasType.Class, "Class_fore_leg", "Class_fore_leg/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_fore_leg");
        SetSlotRegion(AtlasType.Class, "Class_back_leg", "Class_back_leg/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_leg");
        SetSlotRegion(AtlasType.Class, "Class_back_arm1", "Class_back_arm1/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_arm1");
        SetSlotRegion(AtlasType.Class, "Class_back_arm2", "Class_back_arm2/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_arm2");
        SetSlotRegion(AtlasType.Class, "Class_back_coat", "Class_back_coat/" + (isFemale ? "FM_" : "M_") + classType.ToString() + "_back_coat");
    }

    public void ChangeRaceRegion(RaceType raceType)
    {
        SetSlotRegion(AtlasType.Race, "Race_Body", "Body/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_Body");
        SetSlotRegion(AtlasType.Race, "Head", "Head/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_Head");
        SetSlotRegion(AtlasType.Race, "Race_fore_hand", "fore_hand/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_fore_hand");
        SetSlotRegion(AtlasType.Race, "Race_fore_arm1", "fore_arm1/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_fore_arm1");
        SetSlotRegion(AtlasType.Race, "Race_fore_arm2", "fore_arm2/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_fore_arm2");
        SetSlotRegion(AtlasType.Race, "Race_fore_leg", "fore_leg/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_fore_leg");
        SetSlotRegion(AtlasType.Race, "Race_back_leg", "back_leg/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_back_leg");
        SetSlotRegion(AtlasType.Race, "Race_back_arm1", "back_arm1/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_back_arm1");
        SetSlotRegion(AtlasType.Race, "Race_back_arm2", "back_arm2/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_back_arm2");
        SetSlotRegion(AtlasType.Race, "Race_back_hand", "back_hand/" + (isFemale ? "FM_" : "M_") + raceType.ToString() + "_back_hand");
    }

    public Spine.AtlasRegion GetAtlasRegion(AtlasType type, string name)
    {
        Spine.AtlasRegion atlasRegion = null;
        switch (type)
        {
            case AtlasType.Weapon:
                atlasRegion = weaponAtlas.GetAtlas().FindRegion(name);
                break;
            case AtlasType.ClassForeHand:
                atlasRegion = classForeHandAtlas.GetAtlas().FindRegion(name);
                break;
            case AtlasType.Class:
                atlasRegion = classAtlas.GetAtlas().FindRegion(name);
                break;
            case AtlasType.Race:
                atlasRegion = raceAtlas.GetAtlas().FindRegion(name);
                break;
            case AtlasType.Eyes:
                atlasRegion = eyesAtlas.GetAtlas().FindRegion(name);
                break;
            case AtlasType.Hair:
                atlasRegion = hairAtlas.GetAtlas().FindRegion(name);
                break;
        }
        return atlasRegion;
    }

    public void SetSlotRegion(AtlasType atlatType, string slotName, string regionName)
    {
        Spine.Slot slot = skeleton.FindSlot(slotName);
        if (slot == null)
        {
            Debug.LogError("Cannot find slot " + slotName);
            return;
        }
        Spine.AtlasRegion region = GetAtlasRegion(atlatType, regionName);
        if (region == null)
        {
            Debug.LogError("Cannot find region " + regionName);
            return;
        }

        // var attachment = slot.Attachment.GetRemappedClone(region);
        // slot.Attachment = attachment;
        slot.Attachment.SetRegion(region);
        slot.SetToSetupPose();
    }

    public void UpdateSkeletonAnimation()
    {
        skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
        skeletonAnimation.LateUpdate();
        AtlasUtilities.ClearCache();
    }
}
