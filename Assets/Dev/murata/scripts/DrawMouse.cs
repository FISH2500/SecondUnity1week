using System;
using UnityEngine;

public class DrawMouse : MonoBehaviour
{
	[SerializeField] private Camera _camera; // ray���΂����߂̃J����
	[SerializeField] private Area _area; // �Z�b�g���邽�߂̃G���A

	private GameObject _dragObj = null; // ���h���b�O���Ă���I�u�W�F�N�g
	private float _zDistance = 0; // �I����������Z���̈ʒu

	[NonSerialized] public GameObject DrawObject;

	private float _yPos;//�J�[�h��Y�̌Œ�ʒu

	void Update()
	{
		// �N���b�N���ꂽ�u��
		if (Input.GetMouseButtonDown(0))
		{
			// �}�E�X�̉�ʏ�̈ʒu����A�J�����̉��Ɍ������������쐬
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) // �������΂��ē�������
			{
				if (!hit.collider.CompareTag("Card")) return; // Card�ȊO�Ȃ炱��ȏ�s��Ȃ�

				//�叫�̏ꍇ�̓J�[�h��I���ł��Ȃ��悤�ɂ���
				if (hit.collider.gameObject.GetComponent<SetSoldier>().IsGeneral) return;

                GameObject obj = hit.collider.gameObject;

				if (DrawObject == obj)
				{
					// �I�������I�u�W�F�N�g��ۑ�
					_dragObj = obj;
					// Z�̈ʒu��ۑ�
					_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;
					_yPos = obj.transform.position.y;
				}
				else if(_area.AllSet)
				{
					_area.RemoveArea(obj);//�j������J�[�h��I��
					Destroy(obj);
				}

				// ���O�𗬂�
				Debug.Log("�N���b�N�����I�u�W�F�N�g: " + hit.collider.gameObject.name);
			}
		}

		if (_dragObj == null) return;

		// �h���b�O��
		if (Input.GetMouseButton(0))
		{
			Plane plane = new Plane(Vector3.up, new Vector3(0, _yPos, 0));

			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (plane.Raycast(ray, out float enter))
			{
				Vector3 hitPoint = ray.GetPoint(enter);

				_dragObj.transform.position = hitPoint;
			}
		}

		// �w�𗣂�����
		if (Input.GetMouseButtonUp(0))
		{
			if(_area.SetAria(_dragObj))
			{
				TurnManager.instance.ChangeTurn();
				enabled = false;
			}

			_dragObj = null; // �h���b�O���Ă���I�u�W�F�N�g��NULL��
		}
	}

	void SetDestroySelectCardWindow() 
	{
        for (int i = 0; i < _area.CardObj.Length; i++)
        {
			bool isGeneral = _area.CardObj[i].GetComponent<SetSoldier>().IsGeneral;//�J�[�h���叫���ǂ����𔻒�

			if (isGeneral) continue;//�叫�̏ꍇ�̓A�E�g���C�������Ȃ�

            _area.CardObj[i].GetComponent<SetOutLine>().SetOutline(0.03f);//�Z�b�g����Ă��邷�ׂẴJ�[�h�ɃA�E�g���C��������
        }

        _selectDestroyCardUI.SetActive(true);
    }

    void ReSetDestroySelectCardWindow()
    {
        for (int i = 0; i < _area.CardObj.Length; i++)
        {

            bool isGeneral = _area.CardObj[i].GetComponent<SetSoldier>().IsGeneral;//�J�[�h���叫���ǂ����𔻒�

            if (isGeneral) continue;//�叫�̏ꍇ�̓A�E�g���C�������Ȃ�

            _area.CardObj[i].GetComponent<SetOutLine>().ReSetOutline();//�A�E�g���C��������
        }

        _selectDestroyCardUI.SetActive(false);
    }
}