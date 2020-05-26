# spel-for-samarbete-DATX02-20-90
Repo for the Unity project in DATX02-20-90

## För att köra projektet
- Klona repot eller ladda ner en .zip av filerna i repot
- Öppna projektet via Unity Hub (Klicka på "Add" och välj mappen dit du klonade repot)
- Installera Multiplayer HLAPI via Unitys inbyggda package manager (Window -> Package manager -> Sök efter hlapi och klicka installera)
- Ändra byggordningen i build settings så att GameScene ligger på plats **0** och GameOverScene ligger på plats **1** (File -> build settings -> dra in både GameScene och GameOverScene -> se till att GameScene ligger överst)
- Klicka på build eller kör spelet direkt i Unity

## För att installera och köra på iPad
Följ nedanstående guider

- https://www.youtube.com/watch?v=80-nE7ichvk
- https://learn.unity.com/tutorial/building-for-mobile

----

# Gruppspecifikt

## Grundläggande grejer: 
### Pusha aldrig direkt till master!

#### Specifikt för git
- Jobba alltid på branches
- Glöm inte "rebase"
- Skriv commit messages som att du säger till kodbasen vad den ska göra. "**Add** xxx", "**Update** xxx", "**Refactor** xxx" (alltså inte Add**ed** osv.)
- Är du osäker - fråga i gruppen! Hellre en fråga för mycket än att behöva greja med ett förstört repo
- Kolla gruppkontraktet för mer specifika regler

#### Layout
T.ex scripts hör hemma i "Assets/Scripts", samma logik för materials och models
