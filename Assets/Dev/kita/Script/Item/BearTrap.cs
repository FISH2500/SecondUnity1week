using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject _trapPrefab;//㩂̃v���n�u

    private int _spawnTurn;//�������ꂽ�^�[��

    public bool _isCanTrapSet = false;//㩂��Z�b�g���ł���ǂ����̃t���O

    private GameObject _card;//�N���b�N�����J�[�h�̃I�u�W�F�N�g

    private GameObject _trapInstance;//�������ꂽ㩂̃I�u�W�F�N�g
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
<<<<<<< Updated upstream

        SetTrapFlag();
//>>>>>>> Stashed changes
//=======
        //SetTrap();
//>>>>>>> 2b3865e25148541241e6c50271f141be880b6f35
    }
=======
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			SetTrapFlag();
			DispUI.instance.Disp(false);
		}
		else
		{
			_isCanTrapSet = false;
			_spawnTurn = TurnManager.instance.TurnCount;
			ExecuteCPUTrap();
		}
	}
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update()
    {
        //㩂��������ꂽ�^�[������1�^�[���o�߂�����㩂�j������
        if (TurnManager.instance.TurnCount > _spawnTurn)
        {
            _isCanTrapSet = false;//㩂̃Z�b�g���ł��Ȃ��悤�ɂ���
            _card.GetComponent<SetSoldier>().IsTrap = false;//�N���b�N�����J�[�h��㩂̃t���O�����Z�b�g
            Debug.Log("㩂��j������܂���");
            EndTask();
        }

<<<<<<< Updated upstream
        if (_isCanTrapSet)//㩂��Z�b�g�ł���
        SetTrapCard();
=======
        if (_isCanTrapSet)//トラップの設置処理
			SetTrapCard();
>>>>>>> Stashed changes
    }

    //�g���b�v�̃Z�b�g�������鏈��
    public void SetTrapFlag() 
    {
        TextManegar.instance.SetText("㩂��ǂ��ɐݒu���܂����H");
        _isCanTrapSet = true;//㩂��Z�b�g�ł���悤�ɂȂ�
        _spawnTurn = TurnManager.instance.TurnCount;//�������ꂽ�^�[�����L�^
    }

    //�I�u�W�F�N�g�Ƀg���b�v��ݒu���鏈��
    void SetTrapCard() 
    {
        if (Input.GetMouseButton(0))
        {
            _card = ClickObject();//�N���b�N�����J�[�h�𒲂ׂ�

            if (_card == null) return;

            bool isGeneral= _card.GetComponent<SetSoldier>().IsGeneral;

            if (isGeneral) return;//���R�ɂ�㩂��Z�b�g�ł��Ȃ�

            _card.GetComponent<SetSoldier>().IsTrap = true;//�N���b�N�����J�[�h��㩂̃t���O�𗧂Ă�

            _isCanTrapSet = true;//㩂��Z�b�g���ꂽ

            Vector3 trapPosition = _card.transform.position;//�N���b�N�����J�[�h�̈ʒu���擾

            trapPosition.y += 0.2f;//㩂��J�[�h�̏�ɕ\�������悤��Y���W�������グ��

<<<<<<< Updated upstream
            _trapInstance= Instantiate(_trapPrefab, trapPosition, Quaternion.identity);//㩂��N���b�N�����J�[�h�̈ʒu�ɐ���

            _isCanTrapSet = false;//㩂̃Z�b�g�����������̂Ńt���O�����Z�b�g
        }
=======
            _isCanTrapSet = false;//トラップ設置完了
			DispUI.instance.Disp(true);
		}
>>>>>>> Stashed changes
    }

    GameObject ClickObject() 
    {
        Vector3 mousePos = Input.mousePosition;
        Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(selectRay, out hit, 100.0f))
        {
            Debug.Log("hit");
            GameObject hitObj = hit.collider.gameObject;

<<<<<<< Updated upstream
                if (hitObj.CompareTag("Card"))
                {
                    return hitObj;//�N���b�N�����J�[�h�̃I�u�W�F�N�g��Ԃ�
                }
=======
            if (hitObj.CompareTag("Card"))
            {
                return hitObj;//なんのカードを押したかチェック
>>>>>>> Stashed changes
            }
        }

        return null;//�N���b�N�����I�u�W�F�N�g���J�[�h�łȂ��ꍇ��null��Ԃ�

    }

    //�^�[���I�����ɔj��
    void EndTask()
    {
        Destroy(_trapInstance);
        Destroy(gameObject);
    }

	private void ExecuteCPUTrap()
	{
		GameObject[] cpuCards = CPUArea.Instance.CardObject;
		System.Collections.Generic.List<GameObject> candidates = new System.Collections.Generic.List<GameObject>();

		foreach (var obj in cpuCards)
		{
			if (obj != null && !obj.GetComponent<SetSoldier>().IsGeneral)
				candidates.Add(obj);
		}

		if (candidates.Count > 0)
		{
			_card = candidates[Random.Range(0, candidates.Count)];
			_card.GetComponent<SetSoldier>().IsTrap = true;

			Debug.Log($"{_card.name}に罠を設置しました");
		}
	}
}
