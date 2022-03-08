using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mCamera;

    public Transform targetTransform;
    public Transform cameraCenterTransform;

    private float mYOffset = 1.0f;
    private float mSensitivity = 3.0f;

    private float mScrollSensitivity = 5.0f;
    private float mScrollDampening = 6.0f;

    private float mZoomMin = 3.5f;
    private float mZoomMax = 15f;
    private float mZoomDefault = 10f;
    private float mZoomDistance;

    private float mCollisionSensitivity = 4.5f;

    private RaycastHit mCameraHit;
    private Vector3 mCameraDistance;


    private void Start() {
        mCamera = GetComponent<Camera>();

        mCameraDistance = mCamera.transform.localPosition;
        mZoomDistance = mZoomDefault;

        mCameraDistance.z = mZoomDistance;

        Cursor.visible = false;
    }

    private void Update() {
        cameraCenterTransform.position = new Vector3(
            targetTransform.position.x,
            targetTransform.position.y + mYOffset,
            targetTransform.position.z);

        // Calculate rotation being applied to the current mouse position.
        Quaternion rotation = Quaternion.Euler(
            cameraCenterTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * mSensitivity / 2f,
            cameraCenterTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * mSensitivity,
            cameraCenterTransform.rotation.eulerAngles.z);

        cameraCenterTransform.rotation = rotation;

        // Is the player actually scrolling.
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * mScrollSensitivity;
            scrollAmount *= mZoomDistance * 0.3f;

            mZoomDistance += -scrollAmount;

            mZoomDistance = Mathf.Clamp(mZoomDistance, mZoomMin, mZoomMax);
        }

        if (mCameraDistance.z != -mZoomDistance) {
            mCameraDistance.z = Mathf.Lerp(mCameraDistance.z, -mZoomDistance, Time.deltaTime * mScrollDampening);
        }

        mCamera.transform.localPosition = mCameraDistance;

        // Checking for collsion.
        GameObject obj = new GameObject();
        obj.transform.SetParent(mCamera.transform.parent);
        obj.transform.localPosition = new Vector3(
            mCamera.transform.localPosition.x,
            mCamera.transform.localPosition.y,
            mCamera.transform.localPosition.z - mCollisionSensitivity);

        // If there is a collision...
        if (Physics.Linecast(cameraCenterTransform.position, obj.transform.position, out mCameraHit)) {
            mCamera.transform.position = mCameraHit.point;

            Vector3 localPosition = new Vector3(
                mCamera.transform.localPosition.x,
                mCamera.transform.localPosition.y,
                mCamera.transform.localPosition.z + mCollisionSensitivity);

            mCamera.transform.localPosition = localPosition;
        }

        // Destroy the collision check.
        Destroy(obj);

        // Prevent clipping of camera inside collision.
        if (mCamera.transform.localPosition.z > -1f) {
            mCamera.transform.localPosition = new Vector3(mCamera.transform.localPosition.x, mCamera.transform.localPosition.y, -1f);
        }
    }
}
