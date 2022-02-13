using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// UIをドラッグ可能にする
/// EventSystemsよりドラッグインターフェースを利用する。
/// 
/// ポイント
/// タッチ操作の場合は指で隠さないようにやや上の位置に表示させる。
/// </summary>
public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Vector2 _draggingOffset = new Vector2(0.0f, 40.0f);   //ドラッグ中のアイコンのオフセット
    private GameObject _draggingObject; //ドラッグ中のアイコンのゲームオブジェクトを保持
    private RectTransform _canvasRectTransform; //カンバスのRectTransformを保持

    /// <summary>
    /// アピアランス（外観）
    /// </summary>
    void CopyAppearanceByImage(in Image dest, Image src)
    {
        dest.sprite = src.sprite;
        dest.rectTransform.sizeDelta = src.rectTransform.sizeDelta;
        dest.color = src.color;
        dest.material = src.material;
    }

    /// <summary>
    /// ドラッグ開始した時。
    /// 　新規GameObjectを作成したものをドラッグMoveさせる。Imageは元のアイコンからコピーしてつかう。
    /// 　無駄でもあり、リビルドもあり、改善の余地がある。Image => SpriteRendererにしてみる？
    /// </summary>
    /// <param name="eventData"></param>
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(_draggingObject != null)
        {
            Destroy(_draggingObject);
        }

        //元のアイコンのImageを取得する。
        var srcImg = GetComponent<Image>();

        //ドラッグのためのゲームオブジェクトを作成する。（ここにImageをアタッチする）
        _draggingObject = new GameObject("Dragging Object");
        //元のアイコンのCanvasの子要素にして前面に表示させる
        _draggingObject.transform.SetParent(srcImg.canvas.transform);
        _draggingObject.transform.SetAsLastSibling();//最後の子要素とする。
        _draggingObject.transform.localScale = Vector3.one;

        //Canvas Group コンポーネントのBlock Raycasts Propertyを使い、レイキャストがブロックされないようにする。
        var canvasGroup = _draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        //ドラッグ中のゲームオブジェクトにImageコンポーネントをアタッチする。（アイコンImgを付けるため）
        var draggingImg = _draggingObject.AddComponent<Image>();
        //元のアイコンと同じアピアランスを設定する。
        CopyAppearanceByImage(draggingImg, srcImg);

        //カンバスのRectTransformを保持しておく
        _canvasRectTransform = draggingImg.canvas.transform as RectTransform;

        //ドラッグ中のアイコンの位置を更新する
        UpdateDraggingObjectPos(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Destroy(_draggingObject);
    }

    private void UpdateDraggingObjectPos(PointerEventData pointerEventData)
    {
        if(_draggingObject == null)
        {
            return;
        }

        //ドラッグ中のアイコンのスクリーン座標からオフセットを合わせる。
        Vector3 screenPos = pointerEventData.position + _draggingOffset;

        //RectTransformから、スクリーン座標をワールド座標に変換する
        Camera camera = pointerEventData.pressEventCamera;
        if(RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _canvasRectTransform, screenPos, camera
            , out Vector3 newPos))
        {
            //ドラッグ中のアイコンの位置をワールド座標で設定する
            _draggingObject.transform.position = newPos;
            _draggingObject.transform.rotation = _canvasRectTransform.rotation;
        }
    }
}
