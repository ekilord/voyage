using DG.Tweening;
using UnityEngine;

public static class UIUtils
{
    public static void MoveGameObjectAbsolutelyHorizontallyEaseInOut(GameObject gameObject, float targetX, float duration)
    {
        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveX(targetX, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectAbsolutelyVerticallyEaseInOut(GameObject gameObject, float targetY, float duration)
    {
        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveY(targetY, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectAbsolutelyHorizontallyEaseInOut(GameObject gameObject, float targetX, float duration, float delay)
    {
        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveX(targetX, duration).SetEase(Ease.InOutQuad).SetDelay(delay);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectAbsolutelyVerticallyEaseInOut(GameObject gameObject, float targetY, float duration, float delay)
    {
        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveY(targetY, duration).SetEase(Ease.InOutQuad).SetDelay(delay);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectRelativelyHorizontallyEaseInOut(GameObject gameObject, float targetX, float duration)
    {
        float currentX = gameObject.transform.localPosition.x;

        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveX(currentX - targetX, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectRelativelyVerticallyEaseInOut(GameObject gameObject, float targetY, float duration)
    {
        float currentY = gameObject.transform.localPosition.y;

        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveY(currentY - targetY, duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectRelativelyHorizontallyEaseInOut(GameObject gameObject, float targetX, float duration, float delay)
    {
        float currentX = gameObject.transform.localPosition.x;

        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveX(currentX - targetX, duration).SetEase(Ease.InOutQuad).SetDelay(delay);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static void MoveGameObjectRelativelyVerticallyEaseInOut(GameObject gameObject, float targetY, float duration, float delay)
    {
        float currentY = gameObject.transform.localPosition.y;

        if (gameObject != null)
        {
            gameObject.transform.DOLocalMoveY(currentY - targetY, duration).SetEase(Ease.InOutQuad).SetDelay(delay);
        }
        else
        {
            Debug.LogWarning("MoveGameObjectToX: The provided GameObject is null.");
        }
    }

    public static class Colors
    {
        public static Color Gold => ColorFromInt(255, 215, 0);
        public static Color DeepGold => ColorFromInt(255, 140, 0);
        public static Color Red => ColorFromInt(220, 20, 60);
        public static Color Pink => ColorFromInt(255, 105, 180);
        public static Color Purple => ColorFromInt(148, 0, 211);
        public static Color Gray => ColorFromInt(128, 128, 128);
        public static Color LightGray => ColorFromInt(211, 211, 211);
		public static Color DarkRed => ColorFromInt( 139, 0, 0 );
		public static Color Crimson => ColorFromInt( 220, 20, 60 );
		public static Color DeepRed => ColorFromInt( 178, 34, 34 );
		public static Color LightRed => ColorFromInt( 255, 102, 102 );
		public static Color BloodRed => ColorFromInt( 165, 0, 0 );
		public static Color Scarlet => ColorFromInt( 255, 36, 0 );

		public static Color ColorFromInt(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }
    }
}
