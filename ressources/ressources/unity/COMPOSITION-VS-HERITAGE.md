# Composition plûtot qu'héritage

Il est important de comprendre qu'Unity promeut un système de composition au lieu d'un système d'héritage. Autrement dit, au lieu d'écrire des scripts spécialisés pour chaque type d'ennemi qui ont une base commune (héritage). Unity incite plutôt à décomposer les fonctionnalités en plusieurs scripts et les lier à un GameObject en fonction de ses besoins (composition). Par exemple, pour un GameObject de type ennemi pourrait avoir les scripts suivants :
- Un script pour le déplacement
- Un script pour la santé
- Un script pour les attaques
- ...

La composition rend le code beaucoup plus flexible. Elle permet très facilement de réutiliser une fontionnalité ailleurs ou tout simplement d'en ajouter ou en retirer une à l'envie. Ce qui fait qu'avec la structure précédente, nous pourrions avoir :
- Un ennemi qui attaque sans se déplacer
- Un ennemi qui se déplace uniquement
- ...

Car avec l'héritage, l'hybridation de fonctionnalités apporte une complexité et peut nécessiter un nombre conséquent de classes. 

Attention tout de même, l'héritage n'est pas une chose à bannir mais plutôt à utiliser avec parcimonie vu qu'Unity nous propose la composition. Car quoiqu'il arrive, nous utiliserons quand même l'héritage vu que nos scripts hériteront de `MonoBehaviour` et plus tard de `ScriptableObject`. En tous les cas, si ces notions vous semblent complexes, ne vous inquétiez pas, nous pratiquerons (beaucoup) tout ça.
- [En savoir plus la différence entre la composition et l'héritage - anglais](https://gamedevbeginner.com/how-to-use-script-composition-in-unity)

La composition peut également prendre forme avec les interfaces. Pour faire très simple, une interface (souvent préfixée par un i majuscule) est un contrat que passe une classe avec l'interface pour implémenter ses fonctionnalités (souvent des méthodes) et ensuite définir son comportement individuellement. Par exemple, deux GameObjects complètement différents (ennemi et objet, par exemple) peuvent avoir une interface nommée `IDamageable` avec une méthode `TakeDamage(int damage)` qui aura un comportement interne à leur classe respective. Si une classe implémente une interface, elle doit impérativement déclarer **publiquement** toutes les méthodes (et autres) que l'interface déclare. Exemple :
```c#
using UnityEngine;

// On commence par un i majuscule pour indiquer que c'est une interface
public interface IDamageable
{
    // On ne définit que la signature de la méthode, pas son contenu
    void TakeDamage(int damage)
}
```

### Implémentation pour un ennemi
```c#
using UnityEngine;
using System.Collections;

// On indique que la classe passe un contrat avec IDamageable
public class Enemy : MonoBehaviour, IDamageable
{
    // On implémente publiquement la méthode "TakeDamage" avec la même signature
    public void TakeDamage(int damage) {
        DecreaseHealth(damage);
        Debug.Log($"Take {damage}");
    }
}
```

### Implémentation pour un objet
```c#
using UnityEngine;
using System.Collections;

// On indique que la classe passe un contrat avec IDamageable
public class Object : MonoBehaviour, IDamageable
{
    // On implémente publiquement la méthode "TakeDamage" avec la même signature
    public void TakeDamage(int damage) {
        Debug.Log($"Take {damage}");
        CreateLoot();
        Destroy(gameObject);
    }
}
```

Dans le code ci-dessus, nos deux classes (`Enemy` et `Object`) implémentent l'interface `IDamageable` avec la méthode `TakeDamage(int damage)` mais un comportement différent pour chacune des classes. De ce fait, si dans notre jeu, il est possible de donner des coups à des éléments, au lieu de vérifier un à un ce qui a été touché, nous n'avons besoin que de vérifier si l'élément à l'interface `IDamageable` pour ensuite appeler la méthode `TakeDamage(int damage)`.


**A noter qu'une interface ne peut pas être instanciée.** Et que si une classe peut implémenter autant d'interfaces qu'elle le souhaite, elle ne peut pas hériter d'une autre interface. Une interface peut également avoir plusieurs méthodes et propriétés. Enfin, un gameobject ne peut implémenter qu'une seule interface à la fois. Autrement dit, si un `GameObject` possède deux composants (ou plus), ces composants ne peuvent pas implémenter la même interface.
- [En savoir plus sur les interfaces en C# avec Unity - anglais (moins de 5 minutes)](https://www.youtube.com/watch?v=50_qBoKGKxs)

> La partie sur les interfaces n'est là qu'à titre indicatif, nous n'aurons pas l'occasion de les utiliser sauf si votre projet de SAE s'y prête.
