﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

namespace BeatSaberMultiplayer
{
    class AvatarController : MonoBehaviour
    {
        PlayerInfo playerInfo;

        GameObject head;
        GameObject leftHand;
        GameObject rightHand;

        TextMeshPro playerNameText;

        Vector3 targetHeadPos;
        Vector3 lastHeadPos;

        Vector3 targetLeftHandPos;
        Vector3 lastLeftHandPos;

        Vector3 targetRightHandPos;
        Vector3 lastRightHandPos;

        Quaternion targetHeadRot;
        Quaternion lastHeadRot;

        Quaternion targetLeftHandRot;
        Quaternion lastLeftHandRot;

        Quaternion targetRightHandRot;
        Quaternion lastRightHandRot;

        float interpolationProgress = 0f;

        public AvatarController()
        {

            CreateGameObjects();
        }

        void CreateGameObjects()
        {
            if (head == null)
            {
                head = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(head.GetComponent<BoxCollider>());
                head.transform.SetParent(transform);
                head.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            }

            if (leftHand == null)
            {
                leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(leftHand.GetComponent<BoxCollider>());
                leftHand.transform.SetParent(transform);
                leftHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.33f);
            }

            if (rightHand == null)
            {
                rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(rightHand.GetComponent<BoxCollider>());
                rightHand.transform.SetParent(transform);
                rightHand.transform.localScale = new Vector3(0.1f, 0.1f, 0.33f);
            }

            if(playerNameText == null)
            {
                playerNameText = BSMultiplayerUI._instance.CreateWorldText(head.transform,"");
                playerNameText.rectTransform.anchoredPosition3D = new Vector3(0f, 1f, 0f);
                playerNameText.alignment = TextAlignmentOptions.Center;
            }
        }

        void Update()
        {
            if (head != null && leftHand != null && rightHand != null && playerNameText != null)
            {
                interpolationProgress += Time.deltaTime * 20;
                if (interpolationProgress > 1f)
                {
                    interpolationProgress = 1f;
                }

                head.transform.localPosition = Vector3.Lerp(lastHeadPos, targetHeadPos, interpolationProgress);
                leftHand.transform.localPosition = Vector3.Lerp(lastLeftHandPos, targetLeftHandPos, interpolationProgress);
                rightHand.transform.localPosition = Vector3.Lerp(lastRightHandPos, targetRightHandPos, interpolationProgress);

                head.transform.localRotation = Quaternion.Lerp(lastHeadRot, targetHeadRot, interpolationProgress);
                leftHand.transform.localRotation = Quaternion.Lerp(lastLeftHandRot, targetLeftHandRot, interpolationProgress);
                rightHand.transform.localRotation = Quaternion.Lerp(lastRightHandRot, targetRightHandRot, interpolationProgress);
                
                playerNameText.rectTransform.LookAt(InputTracking.GetLocalPosition(XRNode.Head));
                playerNameText.rectTransform.Rotate(0f, 90f+playerNameText.rectTransform.rotation.eulerAngles.y, 0f);
            }
        }

        public void SetPlayerInfo(PlayerInfo _playerInfo,float offset)
        {
            if (_playerInfo == null) return;
            if(head == null || leftHand == null || rightHand == null || playerNameText == null)
            {
                CreateGameObjects();
            }

            playerInfo = _playerInfo;

            if (playerInfo.playerId == BSMultiplayerClient._instance.localPlayerInfo.playerId && playerInfo.playerName == BSMultiplayerClient._instance.localPlayerInfo.playerName)
            {
                head.SetActive(false);
                leftHand.SetActive(false);
                rightHand.SetActive(false);
                playerNameText.gameObject.SetActive(false);
            }
            else
            {
                head.SetActive(true);
                leftHand.SetActive(true);
                rightHand.SetActive(true);
                playerNameText.gameObject.SetActive(true);

                transform.position = new Vector3(offset, 0f, 0f) + BSMultiplayerClient._instance.roomPositionOffset;
                
            }

            interpolationProgress = 0f;

            lastHeadPos = targetHeadPos;
            targetHeadPos = _playerInfo.headPos;

            lastRightHandPos = targetRightHandPos;
            targetRightHandPos = _playerInfo.rightHandPos;

            lastLeftHandPos = targetLeftHandPos;
            targetLeftHandPos = _playerInfo.leftHandPos;

            lastHeadRot = targetHeadRot;
            targetHeadRot = _playerInfo.headRot;

            lastRightHandRot = targetRightHandRot;
            targetRightHandRot = _playerInfo.rightHandRot;

            lastLeftHandRot = targetLeftHandRot;
            targetLeftHandRot = _playerInfo.leftHandRot;

            playerNameText.text = playerInfo.playerName;
        }


    }
}
