using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AnimCurveSO animCurve;
    public TileObjectData tileObjectData;
    public void SetTileData(TileObjectData tileObjectData)
    {
        this.tileObjectData = tileObjectData;
        this.tileObjectData.OnMove += Move;
        this.tileObjectData.OnColorChanged += OnColorChanged;
        this.tileObjectData.OnNumberChanged += SetNumber;
        this.tileObjectData.OnShapeChanged += ShapeChanged;
        this.tileObjectData.OnTelepoerted += Teleportt;
    }
    private void OnDisable() {
         this.tileObjectData.OnMove -= Move;
        this.tileObjectData.OnColorChanged -= OnColorChanged;
        this.tileObjectData.OnNumberChanged -= SetNumber;
        this.tileObjectData.OnShapeChanged -= ShapeChanged;
        this.tileObjectData.OnTelepoerted -= Teleportt;
    }

    private void Teleportt(Vector2 vector2)
    {
        transform.position = vector2;
    }

    private void ShapeChanged(Sprite obj)
    {
        spriteRenderer.sprite = obj;
    }

    private void OnColorChanged(Color color)
    {
        spriteRenderer.color = color;
    }
    private void SetNumber(int number)
    {
        text.text = number.ToString();
    }

    private void Move(Vector2 vector2,MoveType moveType)
    {
        StartCoroutine(MoveRoutine(vector2,moveType));
    }
    IEnumerator MoveRoutine(Vector2 vector2,MoveType moveType)
    {
        tileObjectData.isMoving = true;
        Vector3 endPos = new Vector3(vector2.x, vector2.y, 0);
        IEnumerator moveRoutine = null;
        switch (moveType)
        {
            case MoveType.NoAnim:
                moveRoutine = Teleport(endPos);
                break;
                case MoveType.StraightMoveUnclamped:
                moveRoutine = StraightMoveUnclamped(endPos);
                break;
                case MoveType.StraightMoveClamped:
                moveRoutine = StraightMoveClamped(endPos);
                break;
                case MoveType.JumpMoveUnclamped:
                moveRoutine = JumpMoveUnclamped(endPos);
                break;
                case MoveType.JumpMoveClamped:
                moveRoutine = JumpMoveClamped(endPos);
                break;
        }

        yield return moveRoutine;
        transform.position = endPos;
        tileObjectData.isMoving = false;
    }
    IEnumerator Teleport(Vector3 vector2)
    {
        transform.position = vector2;
        tileObjectData.isMoving = false;
        yield return null;
    }
    IEnumerator JumpMoveUnclamped(Vector3 vector2)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(vector2.x, vector2.y, 0);
        float percentace = 0;
        while (percentace < 1)
        {
            percentace += Time.deltaTime * 5;
            float position = animCurve.animationCurve.Evaluate(percentace);
            transform.position = LerpedMove(startPos, endPos, position, true);
            yield return null;
        }
    }
    private IEnumerator JumpMoveClamped(Vector3 vector2)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(vector2.x, vector2.y, 0);
        float percentace = 0;
        while (percentace < 1)
        {
            percentace += Time.deltaTime * 5;
            float position = animCurve.animationCurve.Evaluate(percentace);
            transform.position = LerpedMove(startPos, endPos, position, false);
            yield return null;
        }
    }
    private IEnumerator StraightMoveUnclamped(Vector3 vector2)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(vector2.x, vector2.y, 0);
        float percentace = 0;
        while (percentace < 1)
        {
            percentace += Time.deltaTime * 5;
            float position = animCurve.animationCurve.Evaluate(percentace);
            transform.position = Vector3.LerpUnclamped(startPos, endPos, position);
            yield return null;
        }
    }
    private IEnumerator StraightMoveClamped(Vector3 vector2)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(vector2.x, vector2.y, 0);
        float percentace = 0;
        while (percentace < 1)
        {
            percentace += Time.deltaTime * 5;
            float position = animCurve.animationCurve.Evaluate(percentace);
            transform.position = Vector3.Lerp(startPos, endPos, position);
            yield return null;
        }
    }

    Vector3 LerpedMove(Vector3 startpos, Vector3 endpos, float lerpValue, bool unclamped)
    {
        Vector3 dir = (endpos - startpos).normalized;
        var t = Vector3.Cross(dir, Vector3.forward);
        Vector3 midPoint = (startpos + endpos) / 2;
        midPoint += t;
        Vector3 a = Vector3.Lerp(startpos, midPoint, lerpValue);
        Vector3 b = Vector3.Lerp(midPoint, endpos, lerpValue);
        if (unclamped) return Vector3.LerpUnclamped(a, b, lerpValue);
        else return Vector3.Lerp(a, b, lerpValue);

    }

}
