## PLAN DE TEST

### État du jeu
**Musique**
Une musique de fond est jouée, avec un volume défini par défaut.

**Temps:**
Lorsque le jeu démarre, une minuterie apparaît dans le coin supérieur droit pour indiquer le temps écoulé.

**Pause le jeu:**
Lorsque le joueur appuie sur la touche de raccourci [ESC], le jeu se met en pause.
Le mot « PAUSE » s'affiche à l'écran, accompagné de deux boutons.
Le chronomètre est mis en pause, les joueurs ne peuvent plus bouger, les ennemis cessent d'attaquer et les ennemis en patrouille arrêtent de se déplacer.
Cliquer sur « Continuer » permet de reprendre la partie, tandis que « Quitter » ramène au menu principal.

### Layout du jeu

// ça, ça aide la personne qui teste, merci
**Cabane: ** 
Il y a 6 cabanes numéroté. 
|          | Articles                   | Gardé par              |
| -------- | -------------------------- | ---------------------- |
| Cabane 1 | 10 balles, 1 assault rifle |                        |
| Cabane 2 | Dossier 1, 1 submachine    | 1 ennemi en alerte     |
| Cabane 3 | 10 balles                  |                        |
| Cabane 4 | 1 clé (près du café)       | 2 ennemi en alerte     |
| Cabane 5 | Dossier 2                  | 2 ennemi en alerte     |
| Cabane 6 | 1 grenade, 1 assault rifle | 1 ennemi en patrouille |
La clé, deux dossiers émettent la lumière.
note: le submachine s'appuie sur la balustrade à l'extérieur de la cabane.

**Port: ** 
Gardé par un ennemi en alerte et un ennemi en patrouille.

**Ennemie en patrouille**
Il y en a encore deux qui patrouillent dans la cour, près des cabanes 2 et 5.

### Gestions de la collecte des objects
Les objets sont collectés lorsque le joueur est à proximité. À ce moment-là, un effet sonore retentit et un texte ou une image correspondante s'affiche sur le HUD pour rappeler qu'un objet a été obtenu.
Les armes à feu et les étuis à balles sont affichés à gauche, tandis que les clés, documents et grenades sont à droite.

Lorsque des balles sont collectées, leur nombre est automatiquement ajouté à l'affichage situé à droite de l'image, représentant le total des munitions disponibles.
La mention « Dans le chargeur » indique le nombre de balles restantes dans le chargeur de l'arme.

Remarque : Il est possible que certains objets ne puissent pas être récupérés. Cependant, après un redémarrage du jeu, ce problème disparaît.
Je n'ai pas trouvé la raison exacte, mais d'après la logique du code, tout semble fonctionner correctement. Cela pourrait être dû au fait que j'ai défini le rayon du character controller du joueur sur 1 pour éviter que les armes téléchargées depuis les magasins d'actifs ne traversent les murs. Cela peut, dans certains cas, empêcher le joueur d'entrer dans la zone de déclenchement de l'objet.

Si deux armes identiques sont collectées, elles ne seront pas ajoutées à la liste des armes.
Il y a deux fusils d'assaut dans la scène de jeu, mais les joueurs ne pourront en posséder qu’un seul.

### Joueur
**Déplacement du joueur**
1. Le joueur peut utiliser les touches A, D, W, S pour se déplacer à gauche, à droite, en avant et en arrière (mouvements sur l’axe X-Z). Il peut également utiliser la souris pour orienter son regard :
1. Déplacer la souris à gauche ou à droite permet de contrôler la rotation du joueur (rotation sur l’axe Y).
Monter ou descendre la souris permet de lever ou baisser la tête (mouvement du regard sur l’axe Y).


**Tir du joueur:**
Le joueur commence le jeu avec un pistolet contenant 10 balles.
Un réticule en forme de croix est affiché au centre de l’écran pour indiquer l’endroit où le joueur peut tirer.
Lorsqu'il tire, une flamme s’échappe de la bouche du canon et un son est émis.

Les effets sonores varient selon les armes :
1. Pistolet : bruit court et sec.
1. Fusil d’assaut : bruit légèrement plus long.
1. Mitraillette : véritable effet sonore d’une mitrailleuse.

Le tir est déclenché par un clic gauche de la souris, ce qui réduit le nombre de balles restantes.

**Gestion des tirs – Pistolet, Mitraillette, Fusil d’assaut**
1. Pistolet : un clic gauche = un tir.
1. Mitraillette : cadence de tir de 600 balles par minute en maintenant le bouton gauche enfoncé.
1. Fusil d’assaut : chaque clic consomme 3 balles à une cadence d’une balle toutes les 2 secondes.

Si le chargeur contient moins de 3 balles, un message s'affiche pour rappeler de recharger.
Lorsqu'il ne reste aucune balle dans le chargeur, un son retentit et le HUD indique qu’il faut recharger.

**Rechargement des balles:**
Appuyer sur la touche R recharge 10 balles par défaut.
1. S’il reste moins de 10 balles ou si le chargeur est presque plein, seules les balles disponibles seront rechargées.
1. En bas à gauche de l’écran, un compteur de munitions affiche le nombre total de balles en stock et le nombre de balles dans le chargeur.
1. Cette information est mise à jour après chaque rechargement.

Pendant le rechargement :
1. Un message "En train de recharger..." s'affiche à l’écran.
1. Un effet sonore est joué.
1. Le rechargement dure 0,5 seconde, après quoi le message disparaît.

Cas particuliers :
1. Plus de balles en stock → message "Il manque des balles...".
1. Chargeur déjà plein → message "Le chargeur est complet".
1. Tentative de tir avec un chargeur vide → message "Il faut recharger..." + effet sonore.

**Tir sur l’ennemi :**
Lorsqu’un ennemi est touché :
1. Il pousse un bref cri.
1. Du sang vert apparaît à l’endroit touché.
Si le tir rate et touche un mur ou le sol, un effet de poussière blanche est généré.

**Dégats sur l'ennemi:**
Chaque ennemi possède 100 points de vie.
La réduction des points de vie dépend du type d’arme utilisée :
1. Pistolet → 25 points
1. Mitraillette → 10 points
1. Fusil d’assaut → 30 points

Les dégâts varient selon la zone touchée :
1. Tête → dégâts multipliés par 4 (un tir de pistolet = mort instantanée).
1. Corps → 100% des dégâts (4 tirs de pistolet pour tuer un ennemi).
1. Membres → 25% des dégâts.

Une barre de vie s'affiche au-dessus de l’ennemi :
1. Vert → points de vie restants.
1. Gris-blanc → points de vie d'origine.
1. Le texte affiche les PV actuels, représentés graphiquement plutôt qu’en chiffres.

**Mort de l'ennemi:**
Lorsqu’un ennemi meurt :

1. Il pousse un long cri.
1. Il disparaît ensuite.
1. Il laisse tomber 10 balles.
1. Si les joueurs tirent rapidement, ils peuvent générer 20 à 30 balles à récupérer.

### Gestion de munitions
En utilisant les touches de raccourci [E] ou [Q], les joueurs peuvent changer d'arme.
Un effet sonore accompagne chaque changement d'arme.

Lors de la première utilisation du pistolet, celui-ci est le seul à contenir des balles. Les autres armes doivent être rechargées manuellement. Veuillez vous référer à la section précédente pour plus de détails.

Lorsqu'une arme est sélectionnée, son arrière-plan devient vert transparent. Les armes non sélectionnées n'ont pas de couleur d'arrière-plan.

### Ennemi 
**Position d'attaque**
1. Lorsqu'un ennemi est trop proche, il se déplace immédiatement vers le joueur et s'arrête à courte distance.
1. S'il détecte un joueur, même dans son dos, il se retourne et attaque.
1. L'ennemi suit toujours le joueur lorsqu'il est dans sa zone de détection.
1. Si le joueur apparaît dans le champ de vision de l'ennemi, ce dernier l'attaque, mais il ne tire pas au-delà d'une certaine distance.
1. Si le joueur quitte la zone de détection, l'ennemi retourne à sa position d'origine.
1. En mode Difficile, le diamètre de détection est 30 % plus grand (modifications définies aux lignes 26-27 de EnemyAttackLogic).

Lorsque l'ennemi attaque :
1. Il lève son arme avec la main gauche et la pointe vers le joueur (effet d'animation).
1. Il tire à une cadence d'un tir toutes les 2 secondes.
1. Chaque tir est accompagné d'un flash blanc et d'un effet sonore.

**Probabilité d'attaque**
La précision des tirs ennemis dépend du niveau de difficulté :
1. Normal : 33 % de chances de toucher le joueur.
1. Difficile : 50 % de chances de toucher le joueur.
(Les valeurs spécifiques sont visibles dans Debug.log() aux lignes 224 et 227 de EnemyLogic).

**Rechargement des ennemis**
1. L'arme de l'ennemi contient 10 balles.
1. Lorsqu'il n'a plus de munitions, il doit recharger, ce qui prend 3 secondes.
1. Un message s'affiche à l'écran indiquant que l'ennemi recharge ses balles.

**Ennemi en patrouille**
L'ennemi suit un itinéraire prédéfini lorsqu'il patrouille.
Si le joueur sort de sa portée d'attaque, il reprend automatiquement sa route.
En mode Difficile, le diamètre de détection est 30 % plus grand (modifications définies à la ligne 97 de EnemyLogic).

**Ennemi dans la cabane 6**
À l'intérieur de la cabane, l'ennemi est en mode patrouille.
En plus des règles classiques de détection (courte distance ou être vu par l'ennemi), l'ennemi attaque immédiatement dès que le joueur entre dans la maison.


### Utilisation de la grenade
1. Lorsqu'une grenade est ramassée, une notification apparaît à l'écran et une icône de mine s'affiche sur le côté droit.
1. Il n'y a qu'une seule grenade dans la scène de jeu, située dans la cabane 6.
1. En appuyant sur la touche de raccourci [G], le joueur lance une grenade, accompagnée d'un effet sonore de lancement.
1. Dès qu'elle touche un objet (ennemi, sol ou autre élément du décor), la grenade explose immédiatement, générant des effets visuels et sonores.
1. L'explosion inflige des dégâts dans un rayon défini.
1. Si un ennemi se trouve suffisamment proche du point d'impact, il est éliminé instantanément.

Le calcul des dégâts repose sur une méthode de déclenchement :
1. Un trigger est généré au moment de l'explosion.
1. Il détecte tous les objets à proximité et mesure la distance entre eux et l'ennemi.
1. Les dégâts sont alors appliqués en fonction de cette distance.

### Vicotire du jeu
Pour terminer le niveau, le joueur doit trouver 2 fichiers et 1 clé.

Lorsqu'il atteint la porte de sortie, le système vérifie son inventaire :
1. Si tous les objets requis sont en sa possession, la porte s'ouvre automatiquement et il peut sortir.
1. Si des documents manquent, une icône de document s'affiche au centre de l'écran, accompagnée d'un message explicatif.

En cas de victoire, un message de félicitations apparaît à l'écran.
Deux options sont proposées :
1. Réessayer
1. Quitter pour revenir à la page principale.

### Éche du jeu
Si les points de vie du joueur tombent à 0, il perd la partie.
L'écran devient rouge, indiquant son échec.
Comme en cas de victoire, deux options sont proposées :
1. Réessayer
1. Quitter pour revenir à la page principale.

