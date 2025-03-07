# ScriptableObjects (abréviation SO)

Un ScriptableObject est un conteneur de données suivant un template que vous aurez défini. Ces templates peuvent contenir un ou plusieurs propriétés de plusieurs types :
- Primitives : entier (int), chaînes de caractères (string)...
- Complexes : GameObject, Transform, Vector2, tableau (array), Sprite...

Ils sont souvent utilisés lorsque vous avez plusieurs objets qui ont la même structure de données. Par exemple, les éléments d'un inventaire dans un jeu vidéo se prêtent bien pour être des ScriptableObjects.

L'un des grands avantages des ScriptableObjects est leur limitation en terme d'empreinte dans la mémoire. En effet, ils limitent la copie d'objets là où un objet MonoBehaviour aurait copié toutes les données autant de fois qu'il aurait été utilisé. 

Par exemple, admettons que nous souhaitons faire un jeu vidéo avec des marchands proposant des bateaux et dont la structure suivante :
![Alt text](bateaux-SO.jpg)
| **name**        | Trois-mâts | Sous-marin |
|-----------------|---------:|------------|
| **cost**        | 6500     | 7800       |
| **damage**      | 80       | 120        |
| **description** | [...]      | [...]        |
| **levelRequired** | 21      | 42        |
| **...** | ...      | ...        |

Si dans votre jeu possède 10 marchands sur la même scène, chacun d'eux aura ses propres instances des mêmes bateaux pour les vendre. Causant ainsi une occupation de mémoire inutile. 

En utilisant les ScriptableObjects chaque bateau instancié ira chercher ses données au même endroit, et ce, quelque soit le nombre de vendeurs sur votre scène. Ainsi que vous ayez 10 ou 1 000 vendeurs, les bateaux proposés occuperont le même espace mémoire (sensiblement).
 
Au-delà de la diminution de l'usage de la mémoire, les ScriptableObjects possèdent les avantages suivants :
- **Ils existent dans les Assets.** Pas de réinitialisation de valeur si on les modifie (dans le jeu ou l'éditeur) et qu'on arrête le mode "Play". Ainsi, il est possible de passer des informations d'une scène à l'autre
    - **Néanmoins les ScriptableObjects ne sont pas un moyen de sauvegarder les données du joueur dans un vrai build**
- **Limitent le couplage entre les GameObjects.** On veut le plus possible limiter le couplage dans le code, ça nous permet d'utiliser un GameObject seul sans en importer d'autres dont on n'en aurait pas besoin dans une autre scène
    - Un exemple souvent utilisé est celui des points de vie du joueur. En utilisant un ScriptableObject pour gérer les points de vie du joueur, des GameObjects peuvent lire la valeur pour s'adapter en fonction : ennemis plus aggressifs, mouvement du joueur plus lents, plus rapides... sans pour autant interconnecter tous ces GameObjects
    > En somme, les ScriptableObjects permettent d'appliquer le MVC dans Unity. Le Modèle étant le ScriptableObject, la Vue vos scènes et le Contrôleur le script C# MonoBehaviour
- **Utilisables par des non-développeurs.** Dans certains studios de jeu, ce sont les game designers qui s'occupent de les créer. Les développeurs les utilisant ensuite dans leur code et bien évidemment développent la structure des données
- **Existent au-delà de la scène.** Un ScriptableObject est très utile pour faire passer les informations d'une scène à l'autre de façon propre. Par exemple, les points de vie du joueur
- **Centralisent les données.** Vu que plusieurs GameObject lisent le même ScriptableObject, il suffit de l'éditer pour appliquer les modifications partout dans votre projet

> **Pourquoi pas un Singleton ?**
> 
> Petit apparté avant de continuer sur les ScriptableObjects, à la lecture de leur description, on pourrait penser qu'un ScriptableObject n'est qu'un Singleton stylisé, non ? Les deux permettent de partager des données entre divers classes, oui. Les deux sont uniques au sein d'un projet, oui. Mais les comparaisons s'arrêtent ici. Avec Unity, un Singleton doit impérativement hériter de `Monobehavior` ce qui pose un problème de couplage entre nos GameObjects. Et c'est avant-tout ce problème que nous essayons de résoudre.
> 
> Pour rappel (dans les grandes lignes), un Singleton est une classe qui ne peut être instanciée qu'une seule fois par projet et dans le cas d'Unity qu'une seule fois par Scène.

Voici un exemple de ScriptableObject

```cs
using UnityEngine;

public class BoatWeaponData : ScriptableObject
{

}
```
Dans le code ci-dessus, la grande différence avec les classes que nous avons faites jusqu'à présent c'est qu'elle hérite de `ScriptableObject`. Pour le reste, ça fonctionne plus ou moins comme avant, on définit des propriétés publiques ou privées ainsi que des méthodes, elles aussi à niveau de visibilité variable (private / public). Notez tout de même que les méthodes `Update()` ou `Awake()` ne sont pas utilisables avec un ScriptableObject néanmoins vous pouvez utiliser la méthode `Start()` ou encore référencer un ScriptableObject dans un autre ScriptableObject.

Sinon, si on reprend le cas de nos bateaux en ScriptableObject, nous voulons avoir les informations suivantes pour chaque bateau :
- damage (int) - dégâts
- cost (int) - prix
- name (string) - nom
- description (string) - description
- sprite (Sprite) - Image

```cs
using UnityEngine;

public class BoatWeaponData : ScriptableObject
{
    public int damage;

    // L'attribut [Range(Min, Max)] permet d'avoir, à la place d'un champ, un slider permettant de faire varier une valeur entre les deux bornes précisées incluses. Ceci équivaut en HTML à "<input type="range" min="0" max="1500" />"
    // A noter que cet attribut fonctionne également avec les float
    [Range(0, 1500)]
    public int cost;
    public string name;

    // L'attribut [Multiline] permet d'afficher une zone de texte multiligne dans l'inspecteur Unity, un peu comme <textarea> en HTML
    [Multiline]
    public string description;
    public Sprite sprite;
}
```

Une fois notre ScriptableObject définit, vous pouvez créer autant d'armes que vous le souhaitez. Attention tout de même, le code ci-dessous ne vous permettra pas de créer un ScriptableObject pour le moment car il nous manque l'attribut `CreateAssetMenu()` avant la définition de notre classe. Ce qui nous donne, au final, le code suivant :
```cs
[CreateAssetMenu(fileName = "New BoatWeaponData", menuName = "ScriptableObjects/BoatWeaponData")]
using UnityEngine;

public class BoatWeaponData : ScriptableObject
{
    public int damage;
    [Range(0, 1500)]
    public int cost;
    public string name;
    [Multiline] 
    public string description;
    public Sprite sprite;
}
```
Rapide explication sur la ligne de code que nous venons d'ajouter :
- CreateAssetMenu() : Méthode propre à Unity permettant d'ajouter des nouveaux éléments au menu `Assets > Create`. Ici notre ScritableObject
- fileName : Nom par défaut lors de la création du ScriptableObject. Le nom peut être changé
- menuName : Endroit où va se trouver la création de notre ScriptableObject dans le menu `Assets > Create`
    - Notez que ce chemin doit être unique, plusieurs ScriptableObject ne peuvent pas avoir le même chemin

Etant donné qu'un GameObject ne peut pas utiliser un ScriptableObject directement, nous passerons par une classe `Monobehaviour` pour gérer le tout.

> Note : Comme les variables ou les classes, il ne peut pas avoir plusieurs ScriptableObjects avec le même nom, toutefois il peut en avoir du même type. Dans le cas de nos bateaux, on peut donc avoir notre Trois-mâts et notre Sous-Marin qui utilisent la même base de données. Un peu comme dans une table MySQL où chaque ligne représente une ScriptableObject.

```cs
using UnityEngine;

public class BoatWeaponItemStore : MonoBehaviour
{ 
    // Référence à notre ScriptableObject
    public BoatWeaponData boatWeaponData;

    void Update()
    {
        // Clic gauche
        if(Input.GetMouseButtonDown(0)) {
            Debug.Log(boatWeaponData.name); 
            Debug.Log(boatWeaponData.description); 
            Debug.Log(boatWeaponData.cost);
            Debug.Log(boatWeaponData.description);
            Debug.Log(boatWeaponData.sprite);
        }
    }
}
```
Voilà, avec notre code, nous pouvons afficher les données de notre arme lorsque nous cliquerons dessus. Chaque arme (GameObject) ayant son propre ScriptableObject avec ses données.

Plus d'informations sur les ScriptableObjects :
- https://www.youtube.com/watch?v=PVOVIxNxxeQ
- https://www.youtube.com/watch?v=q81A6cjdGcY - français
- https://docs.unity3d.com/Manual/class-ScriptableObject.html
- https://www.youtube.com/watch?v=WLDgtRNK2VE
- https://learn.unity.com/tutorial/introduction-to-scriptableobjects?language=en#60661f13edbc2a001f55c22b
- https://unity.com/how-to/scriptableobjects-event-channels-game-code
- https://unity.com/how-to/architect-game-code-scriptable-objects

Dans le cas de notre projet d'initiation, nous avons déjà défini des ScriptableObjects, pour gagner du temps, vous allez les récupérer ici :
- [Télécharger les ScriptableObjects du TP d'initiation](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2FDanYellow%2Fcours%2Ftree%2Fmain%2Fcreation-et-design-interactif-s4%2Ftravaux-pratiques%2Fnumero-1%2Fsamples%2Fbeginner-base%2FAssets%2FScripts%2FScriptableObjects)

Désarchivez le tout puis glissez le dossier dans le dossier `Assets/Scripts/` de votre projet.

Nous avons vu les ScriptableObjects pour stocker des données, sachez qu'il est également possible de les utiliser pour créer des évenements (il y en a dans l'archive que vous venez de récupérer), et ce, pour les mêmes raisons que vues précédemment : limitation du couplage du code...
