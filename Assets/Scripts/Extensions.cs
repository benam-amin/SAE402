using UnityEngine;
using System.Collections;

// https://learn.unity.com/tutorial/extension-methods#
public static class Extensions
{
    /// <summary>
    /// Makes a GameObject move back and forth between its initial position and a defined position
    /// </summary>
    /// <param name="transform">GameObject's transform</param>
    /// <param name="endPosition">End position</param>
    /// <returns>IEnumerator</returns>
    public static IEnumerator MoveBackAndForth(this Transform transform, Vector3 endPosition, float duration = 0.125f)
    {
        Vector3 initialPosition = transform.localPosition;

        yield return Move(transform, initialPosition, endPosition);
        yield return Move(transform, endPosition, initialPosition);
    }

    /// <summary>
    /// Moves a GameObject between two positions
    /// </summary>
    /// <param name="transform">GameObject's transform</param>
    /// <param name="from">Initial position</param>
    /// <param name="to">End position</param>
    /// <returns>IEnumerator</returns>
    public static IEnumerator Move(this Transform transform, Vector3 from, Vector3 to, float duration = 0.125f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

    /// <summary>
    /// Allow to make a string bold
    /// Example : "example text in bold".Bold();
    /// </summary>
    /// <param name="str">The string to bold</param>
    /// <returns>String bold</returns>
    public static string Bold(this string str) => "<b>" + str + "</b>";
    /// <summary>
    /// Change the color of a string
    /// Example : "example in Color".Color("red") or "example in Color".Color("FF7400");
    /// </summary>
    /// <param name="str">The string to color</param>
    /// <returns>String colored</returns>
    public static string Color(this string str, string color) => string.Format("<color={0}>{1}</color>", color, str);
    /// <summary>
    /// Make a string italic
    /// Example : "example in Color".Italic();
    /// </summary>
    /// <param name="str">The string to make italic</param>
    /// <returns>String in italic</returns>
    public static string Italic(this string str) => "<i>" + str + "</i>";
    /// <summary>
    /// Change size of a string
    /// Example : "example in Color".Size(10);
    /// </summary>
    /// <param name="str">The string to change size</param>
    /// <returns>String with a size changed</returns>
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);

}