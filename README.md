# Game of Life

## Conception

Projet séparé en deux morceaux:
* Le moteur (backend)
* La visualisation WPF (Frontend)

Le moteur fonctionne egalement en ligne de commande. le Frontend récupère chaque itération et l'affiche.

## WPF

Une grille dynamique 2D permet de prendre le plus d'espace possible à l'écran grâce à des ItemsControl. Dans cette grille, il y a des rectangles avec des événements sur l'activité de la souris pour sélectionner les cellules souhaitées.

L'application permet de lancer, mettre en pause, stopper ou redémarrer totalement la simulation. La taille de la grille est modifiable et s'ajuste par rapport à la fenêtre. On peut également modifier la vitesse d'exécution des itérations.

La couleur des cellules est modifiables sur toutes les couleurs RGB.

L'option de sauvegarde et recharge d'une partie à partir d'un fichier .gol est possible.

Ce projet utilise Extended WPF Toolkit™ qui est permet d'avoir des components en plus. IntUpDown et ColorPicker sont utilisés dans ce projet.