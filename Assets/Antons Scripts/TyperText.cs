using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



//Icke fungerande script s� komenterade inte detta!!! (Anton)



[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class TyperText : MonoBehaviour
{

    private const float PrintDelaySetting = 0.02f;

    private const float PunctuationDelayMultiplier = 8f;

    private static readonly List<char> punctutationCharacters = new List<char>
        {
            '.',
            ',',
            '!',
            '?'
        };

    [SerializeField]
    [Tooltip("The library of ShakePreset animations that can be used by this component.")]
    private ShakeLibrary shakeLibrary;

    [SerializeField]
    [Tooltip("The library of CurvePreset animations that can be used by this component.")]
    private CurveLibrary curveLibrary;

    [SerializeField]
    [Tooltip("If set, the typer will type text even if the game is paused (Time.timeScale = 0)")]
    private bool useUnscaledTime;
#pragma warning restore 0649

    [SerializeField]
    [Tooltip("Event that's called when the text has finished printing.")]
    private UnityEvent printCompleted = new UnityEvent();

    [SerializeField]
    [Tooltip("Event called when a character is printed. Inteded for audio callbacks.")]
    private CharacterPrintedEvent characterPrinted = new CharacterPrintedEvent();

    private TextMeshProUGUI textComponent;
    private float defaultPrintDelay;
    private List<TypableCharacter> charactersToType;
    private List<TextAnimation> animations;
    private Coroutine typeTextCoroutine;

    public UnityEvent PrintCompleted
    {
        get
        {
            return this.printCompleted;
        }
    }

    public CharacterPrintedEvent CharacterPrinted
    {
        get
        {
            return this.characterPrinted;
        }
    }

    public bool IsTyping
    {
        get
        {
            return this.typeTextCoroutine != null;
        }
    }

    private TextMeshProUGUI TextComponent
    {
        get
        {
            if (this.textComponent == null)
            {
                this.textComponent = this.GetComponent<TextMeshProUGUI>();
            }

            return this.textComponent;
        }
    }

    public void TypeText(string text, float printDelay = -1)
    {
        this.CleanupCoroutine();


        foreach (var anim in GetComponents<TextAnimation>())
        {
            Destroy(anim);
        }

        this.defaultPrintDelay = printDelay > 0 ? printDelay : PrintDelaySetting;
        this.ProcessTags(text);


        var textInfo = this.TextComponent.textInfo;
        textInfo.ClearMeshInfo(false);

        this.typeTextCoroutine = this.StartCoroutine(this.TypeTextCharByChar(text));
    }

    public void Skip()
    {
        this.CleanupCoroutine();

        this.TextComponent.maxVisibleCharacters = int.MaxValue;
        this.UpdateMeshAndAnims();

        this.OnTypewritingComplete();
    }

    public bool IsSkippable()
    {
        return this.IsTyping;
    }

    private void CleanupCoroutine()
    {
        if (this.typeTextCoroutine != null)
        {
            this.StopCoroutine(this.typeTextCoroutine);
            this.typeTextCoroutine = null;
        }
    }

    private IEnumerator TypeTextCharByChar(string text)
    {
        this.TextComponent.text = TextTagParser.RemoveCustomTags(text);
        for (int numPrintedCharacters = 0; numPrintedCharacters < this.charactersToType.Count; ++numPrintedCharacters)
        {
            this.TextComponent.maxVisibleCharacters = numPrintedCharacters + 1;
            this.UpdateMeshAndAnims();

            var printedChar = this.charactersToType[numPrintedCharacters];
            this.OnCharacterPrinted(printedChar.ToString());

            if (this.useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(printedChar.Delay);
            }
            else
            {
                yield return new WaitForSeconds(printedChar.Delay);
            }
        }

        this.typeTextCoroutine = null;
        this.OnTypewritingComplete();
    }

    private void UpdateMeshAndAnims()
    {
        this.TextComponent.ForceMeshUpdate();


        for (int i = 0; i < this.animations.Count; i++)
        {
            this.animations[i].AnimateAllChars();
        }
    }
    private void ProcessTags(string text)
    {
        this.charactersToType = new List<TypableCharacter>();
        this.animations = new List<TextAnimation>();
        var textAsSymbolList = TextTagParser.CreateSymbolListFromText(text);

        int printedCharCount = 0;
        int customTagOpenIndex = 0;
        string customTagParam = "";
        float nextDelay = this.defaultPrintDelay;
        foreach (var symbol in textAsSymbolList)
        {
            if (symbol.IsTag && !symbol.IsReplacedWithSprite)
            {
                if (symbol.Tag.TagType == TextTagParser.CustomTags.Delay)
                {
                    if (symbol.Tag.IsClosingTag)
                    {
                        nextDelay = this.defaultPrintDelay;
                    }
                    else
                    {
                        nextDelay = symbol.GetFloatParameter(this.defaultPrintDelay);
                    }
                }
                else if (symbol.Tag.TagType == TextTagParser.CustomTags.Anim ||
                         symbol.Tag.TagType == TextTagParser.CustomTags.Animation)
                {
                    if (symbol.Tag.IsClosingTag)
                    {
                        TextAnimation anim = null;
                        if (this.IsAnimationShake(customTagParam))
                        {
                            anim = gameObject.AddComponent<ShakeAnimation>();
                            ((ShakeAnimation)anim).LoadPreset(this.shakeLibrary, customTagParam);
                        }
                        else if (this.IsAnimationCurve(customTagParam))
                        {
                            anim = gameObject.AddComponent<CurveAnimation>();
                            ((CurveAnimation)anim).LoadPreset(this.curveLibrary, customTagParam);
                        }
                        else
                        {

                        }

                        anim.UseUnscaledTime = this.useUnscaledTime;
                        anim.SetCharsToAnimate(customTagOpenIndex, printedCharCount - 1);
                        anim.enabled = true;
                        this.animations.Add(anim);
                    }
                    else
                    {
                        customTagOpenIndex = printedCharCount;
                        customTagParam = symbol.Tag.Parameter;
                    }
                }
                else
                {

                }

            }
            else
            {
                printedCharCount++;

                TypableCharacter characterToType = new TypableCharacter();
                if (symbol.IsTag && symbol.IsReplacedWithSprite)
                {
                    characterToType.IsSprite = true;
                }
                else
                {
                    characterToType.Char = symbol.Character;
                }

                characterToType.Delay = nextDelay;
                if (punctutationCharacters.Contains(symbol.Character))
                {
                    characterToType.Delay *= PunctuationDelayMultiplier;
                }

                this.charactersToType.Add(characterToType);
            }
        }
    }

    private bool IsAnimationShake(string animName)
    {
        return this.shakeLibrary.ContainsKey(animName);
    }

    private bool IsAnimationCurve(string animName)
    {
        return this.curveLibrary.ContainsKey(animName);
    }

    private void OnCharacterPrinted(string printedCharacter)
    {
        if (this.CharacterPrinted != null)
        {
            this.CharacterPrinted.Invoke(printedCharacter);
        }
    }

    private void OnTypewritingComplete()
    {
        if (this.PrintCompleted != null)
        {
            this.PrintCompleted.Invoke();
        }
    }

    [System.Serializable]
    public class CharacterPrintedEvent : UnityEvent<string>
    {
    }

    private class TypableCharacter
    {
        public char Char { get; set; }

        public float Delay { get; set; }

        public bool IsSprite { get; set; }

        public override string ToString()
        {
            return this.IsSprite ? "Sprite" : Char.ToString();
        }
    }
}
