# Installation Unity

Unity est un logiciel "gratuit" permettant de développer des jeux vidéo aussi bien 2D que 3D. Vous pouvez le télécharger sur le site officiel. Il vous faudra créer un compte, créez un compte personnel.

- [Télécharger Unity Hub](https://unity.com/fr/download)
> Note : Sur le site, vous ne pouvez pas télécharger Unity directement, il faut técharger Unity Hub qui téléchargera Unity pour vous. Dans le cadre du cours, nous utiliserons laa dernière version disponible.

Une fois Unity Hub installé et lancé. Allez dans la rubrique "Installs" et cliquez sur le bouton "Install Editor".
| ![](printscreens/installation-1.jpg) |
|:--:|
| *Dans cette capture, nous avons déjà des versions d'Unity d'installées (3). Il est possible d'avoir plusieurs versions d'Unity sur son ordinateur* |

Une fenêtre va s'ouvrir dans Unity, sélectionnez le bouton "Install" de la dernière version d'Unity proposée...
![](printscreens/installation-2.png)

...et une autre fenêtre va s'ouvrir. Elle demande ce que vous souhaitez installer en plus de la version d'Unity sélectionnée à l'étape précédente.

| ![](printscreens/installation-3.png) |
|:--:|
| *Pour gagner du temps au niveau du téléchargement, décochez le téléchargement de "Microsoft Visual Studio". Nous n'en aurons pas besoin pour le cours. Nous utiliserons VS Code à la place.* |

# Unity à l'IUT

Unity ainsi que Unity Hub sont déjà installés sur la plupart des ordinateurs de l'IUT. Il n'est pas utile de l'installer vous-même. Néanmoins, Unity Hub n'est pas forcément conscient qu'Unity est là, vous devrez le faire manuellement depuis Unity Hub. Suivez les étapes.

| ![](printscreens/locate-unity-1.png) |
|:--:|
| *Cliquez sur le menu "Installs" puis le bouton "Locate"* |

| ![](printscreens/locate-unity-2.png) |
|:--:|
| *Allez dans le dossier "Programmes" et sélectionnez le dossier de la dernière version d'Unity. Puis choississez le fichier "Unity.exe" situé dans `Unity 6.X.XX > Editor`* |

Après avoir effectué ces étapes, vous pourrez utiliser la version d'Unity que vous venez d'ajouter soit pour créer un projet soit pour ouvrir un projet existant.

## Problèmes de connexion
Dans certains cas, il se peut que vous n'arriviez pas à vous connecter à votre compte Unity. Unity Hub arguant que votre licence a expiré et si vous tentez de la recréer, ceci échoue. Ce problème est lié au Pare-Feu de Windows qui peut bloquer les requêtes d'Unity sur certains réseaux comme celui de l'IUT. Si ceci arrive, il y a un didacticiel sur le site d'Unity.
- [Accéder au didacticiel](https://docs.unity3d.com/Packages/com.unity.live-capture@2.0/manual/setup-network.html#manual-firewall-rule-configuration)

Une fois l'opération effectuée, pensez à vous déconnecter d'Unity Hub puis vous reconnecter.

# Configuration d'Unity et VS Code
Il est possible de développer en C# avec n'importe quel logiciel. Toutefois, nous vous recommendons d'utiliser un logiciel qui gère Unity, ceci vous permettra d'avoir l'auto-complétion des différentes méthodes et classes pour ainsi être plus productif et éviter les erreurs.

> Si vous utilisez Visual Studio (pas VS Code, c'est différent) tout sera géré nativement après avoir installé Unity, il faut quand même sélectionner les extensions liées à Unity lors de l'installation.

Pour définir VS Code comme IDE par défaut pour Unity. Suivez les étapes suivantes :
- MacOS :
  - Cliquez sur `Unity` en haut à gauche dans la barre de statut
  - Sélectionnez `Settings`, une fenêtre va apparaître
  - Sélectionnez `External Tools` dans le menu à gauche
  - Sélectionnez la liste déroulante du choix `External Script Editor` et choissiez votre VS Code
- Windows :
  - Cliquez sur le menu `Edit > Preferences`
  - Sélectionnez `External Tools` dans le menu à gauche
  - Sélectionnez la liste déroulante du choix `External Script Editor` et choissiez votre IDE VS Code

[Plus d'informations ici](https://learn.unity.com/tutorial/set-your-default-script-editor-ide#)

Si tout a bien été pris en compte, votre IDE devrait s'ouvrir si vous allez dans le menu : `Assets > Open C# Project`.

# Configuration de C# et VS Code
Par défaut, VS Code ne connaît pas les méthodes propres à Unity. Unity propose beaucoup de classes en tout genre et il est facile de faire une erreur de frappe causant ainsi une erreur à la compilation. Pour vous éviter ce problème, il faudra installer le SDK .NET qui est disponible gratuitement sur le site de Microsoft.

- [Télécharger le SDK .NET](https://dot.net/core-sdk-vscode)

> **IMPORTANT** : Vous devez effectuer l'installation avec le logiciel VS Code **fermé**. Sinon l'installation échouera de façon silencieuse et vous devrez recommencer en n'oubliant pas de désinstaller le SDK .NET avant.


Il faudra également installer des extensions pour VS Code gratuites :
- [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- [Unity](https://marketplace.visualstudio.com/items?itemName=VisualStudioToolsForUnity.vstuc)
- [Unity Code Snippets](https://marketplace.visualstudio.com/items?itemName=kleber-swf.unity-code-snippets)
<br>

Et pour vous assurer que vous avez bien l'auto-complétion des méthodes d'Unity commencez à écrire "OnColli", votre logiciel devrait vous faire des propositions dont "OnCollisionExit2D" ou "OnCollisionEnter".

| ![](printscreens/installation-4.png) |
|:--:|
| *Ici, VS Code me propose des méthodes quand je commence à écrire. Ceci signifie que le SDK .NET s'est bien installé* |

- [En savoir plus - vidéo en anglais moins de 5 minutes](https://www.youtube.com/watch?v=Du0UMIw_EE8)

# Utilisation de git
Pour entretenir vos connaissances de git, vous **devrez mettre l'avancée des cours sur github,** et ce, dès le premier cours. L'avancement de chaque TP devra être enregistré sur GitHub. Par conséquent, créez dès maintenant un nouveau projet sur github en public. Et après avoir cloné ce dépôt sur votre ordinateur, vous créerez votre projet Unity dedans.

Une note sera donnée à la fin des TP concernant votre gestion de git.

Pensez à commiter régulièrement, c'est gratuit. Un commit vous permet d'avoir un point de sauvegarde, vous permettant ainsi de revenir en arrière aisément en cas de problème. Faites-le à chaque étape clé : ajout de ressource / script / changement de structure de niveau...

N'oubliez pas de mettre à la racine de votre projet Unity, un fichier .gitignore, ceci vous évitera de mettre sur git des fichiers de dépendences qui n'ont rien à faire sur votre dépôt. Vous pouvez le récupérer sur le dépôt git [gitignore](https://github.com/github/gitignore).

Prenez celui dédié à Unity (Unity.gitignore) et **pensez bien à renommer le fichier en ".gitignore"**
