using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Sprite backSprite;
    public Sprite Normalbackground;
    public Sprite Goldbackground;
    public Sprite DefaultOutline;
    public Sprite SmallJockerOutline;
    public Sprite BigJockerOutline;
    public Sprite SmallJocker;
    public Sprite BigJocker;
    public Sprite ScatterCard;
    public Sprite [] cardSprites;

    [SerializeField]float normalCardProbability = 0.6f; // Adjust this value as needed to prioritize normal cards
    [SerializeField] float goldenCardProbability = 0.1f;
    [SerializeField] float scatterCardProbability = 0.1f;

    public List<int> RandomSprite = new List<int>();

    float originalScatterprobability = 0.0f;

    [Range(0.0f , 1.0f)]
    public float probabilityFactor; // This is the actual float value

    private void Start ()
    {
        originalScatterprobability = scatterCardProbability;
    }

    //selecting a random Card
    public Sprite RandomizeCardWithProbability ()
    {
        return cardSprites [GetRandomSprite(probabilityFactor)];
    }

    int GetRandomSprite ( float probabilityFactor )
    {
        int random;

        // Ensure we only generate a new random index if we haven't selected enough unique sprites
        if (RandomSprite.Count >= cardSprites.Length)
        {
            RandomSprite.Clear();
        }

        // Adjust the selection process based on the probability factor
        if (RandomSprite.Count > 0 && Random.Range(0f , 1f) < probabilityFactor)
        {
            // Prefer a similar card from the last selected
            random = RandomSprite [Random.Range(0 , RandomSprite.Count)];
        }
        else
        {
            do
            {
                random = Random.Range(0 , cardSprites.Length);
            } while (RandomSprite.Contains(random));

            RandomSprite.Add(random); // Add the selected random index to the list

            // Limit the size of the RandomSprite list to the last 5 selected indices
            if (RandomSprite.Count > 7)
            {
                RandomSprite.RemoveAt(0); // Remove the oldest index
            }
        }

        return random; // Return the randomly picked sprite index
    }



    public Sprite WhichCard (int which)
    {
        return cardSprites [which];
    }

    public void DealSpecificCardType(Transform card,int which)
    {
        card.GetComponent<Card>().SetCardBack(Normalbackground);

        card.GetComponent<Card>().SetCard(WhichCard(which));
        card.GetComponent<Card>().SetCardType();
    }

    public void DealNormalCards(Transform card )
    {
        card.GetComponent<Card>().SetCardBack(Normalbackground);

        card.GetComponent<Card>().SetCard(RandomizeCardWithProbability());
        card.GetComponent<Card>().SetCardType();
    }

    public void DealGoldenCards(Transform card )
    {
        card.GetComponent<Card>().SetCard(RandomizeCardWithProbability());
        card.GetComponent<Card>().SetCardType();
        card.GetComponent<Card>().SetCardBackGolden(Goldbackground);
        card.GetComponent<Card>().SetCardOutLine(DefaultOutline);
    }

    public void DealScatterCards(Transform card )
    {
        card.GetComponent<Card>().SetScatterCard(ScatterCard);
        card.GetComponent<Card>().SetCardType();
    }

    public void DealBigJocker(Transform card )
    {
        card.GetComponent<Card>().SetBigJocker(BigJocker);
        card.GetComponent<Card>().SetCardOutLine(BigJockerOutline);
        card.GetComponent<Card>().SetCardType();
    }

    public void DealSmallJocker(Transform card )
    {
        card.GetComponent<Card>().SetSmallJocker(SmallJocker);
        card.GetComponent<Card>().SetCardOutLine(SmallJockerOutline);
        card.GetComponent<Card>().SetCardType();
    }

    public void RandomizeDealing_golden ( Transform card )
    {
        float normalCardProbability = 0.8f;

        float randomValue = Random.value;

        if (randomValue < normalCardProbability)
        {
            // Deal a normal card
            DealNormalCards(card);
        }
        else
        {
            // Deal a golden card
            DealGoldenCards(card);
        }
    }

    public void RandomizeDealing_Jocker ( Transform card )
    {
        float normalCardProbability = 0.9f;

        float randomValue = Random.value;

        if (randomValue < normalCardProbability)
        {
            // Deal a normal card
            DealSmallJocker(card);
        }
        else
        {
            // Deal a ScatterCard
            DealBigJocker(card);
        }
    }

    public void RandomizeDealing ( Transform card )
    {
        float randomValue = Random.value;

        // Precompute thresholds to avoid redundant calculations
        float goldenThreshold = normalCardProbability + goldenCardProbability;
        float scatterThreshold = goldenThreshold + scatterCardProbability;

        if (CommandCentre.Instance.FreeGameManager_.IsFreeGame)
        {
            scatterCardProbability = 0.01f;
        }
        else
        {
            scatterCardProbability = originalScatterprobability;
        }
      

        if (randomValue < normalCardProbability)
        {
            // Deal a normal card
            DealNormalCards(card);
        }
        else if (randomValue < goldenThreshold)
        {
            // Deal a golden card
            DealGoldenCards(card);
        }
        else if (randomValue < scatterThreshold)
        {
            // Deal a scatter card
            DealScatterCards(card);
        }
    }


    public string CheckCardSpriteAndSetCardType (Transform Card)
    {
        string cardName = "";
        cardName = Card.GetComponent<Card>().card.sprite.name;
        return cardName;
    }

    public void GetAndAssignSprites ( Transform cardTransform )
    {
        // Cache the Card component
        Card cardComponent = cardTransform.GetComponent<Card>();

        // Get all SpriteRenderers attached to the Card object or its children
        SpriteRenderer [] spriteRenderers = cardTransform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            // Check if the spriteRenderer has no sprite assigned
            if (spriteRenderer.sprite == null)
            {
                switch (spriteRenderer.name)
                {
                    case "Back":
                        spriteRenderer.sprite = backSprite;
                        break;

                    case "cardBg":
                        if (cardComponent.IsGoldenCard) 
                        {
                            spriteRenderer.sprite = Goldbackground;
                        }
                        else if (( cardComponent.IsBigJocker || cardComponent.IsSmallJocker ) && cardComponent.IsGoldenCard)
                        {
                            spriteRenderer.sprite = Goldbackground;
                        }
                        else
                        {
                            spriteRenderer.sprite = Normalbackground;
                        }
                        break;

                    case "card":
                        if (cardComponent.IsScatterCard)
                        {
                            spriteRenderer.sprite = ScatterCard;
                        }
                        else if (cardComponent.IsSmallJocker)
                        {
                            spriteRenderer.sprite = SmallJocker;
                        }
                        else if (cardComponent.IsBigJocker)
                        {
                            spriteRenderer.sprite= BigJocker;
                        }
                        else
                        {
                            AssignCardSprite(spriteRenderer , cardComponent.cardType);
                        }
                        break;

                    case "Outline":
                        if (cardComponent.IsGoldenCard)
                        {
                            spriteRenderer.sprite = DefaultOutline;
                        }
                        else if(cardComponent.IsGoldenCard && cardComponent.IsBigJocker)
                        {
                            spriteRenderer.sprite = BigJockerOutline;
                        }
                        else if(cardComponent.IsGoldenCard && cardComponent.IsSmallJocker)
                        {
                            spriteRenderer.sprite = SmallJockerOutline;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void AssignCardSprite ( SpriteRenderer spriteRenderer , CardType cardType )
    {
        // Iterate through the cardSprites array and find the matching sprite by name
        foreach (Sprite cardSprite in cardSprites)
        {
            if (cardSprite.name == cardType.ToString())
            {
                spriteRenderer.sprite = cardSprite;
                break;  // Exit the loop once the correct sprite is found
            }
        }
    }


}