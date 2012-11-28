Spillet er en 3d labyrint explorer med en slags historie bak. Man beveger seg rundt med wasd keys og styrer retningen med musa p� "first person" vis. M�let med spillet er explore seg frem og � l�se g�ter/puzzles underveis, kan ikke si for mye mer uten � spoile "historien" ;)

3D modellene er hentet fra forskjellige kilder, samt jeg har laget noe selv (alt som ser forferdelig ut alts�). Mange modeller og lydfiler har copyrights, er lagt ved en note om det i content folderen.

Jeg m� nevne at jeg i denne innleveringen har brydd meg veldig mye mindre om kodekvalitet enn jeg vanligvis gj�r, for � f� tid til � f� med nok content og ha et kult gameplay. Dette er slik jeg har skj�nt det i hennhold til slik vi skal gj�re det i dette faget, men jeg nevner det likevel da jeg er klar over at kodekvaliteten langt i fra er p� topp.

Selve grunn-kontroller mekanismen i spillet (kameraet), samt veggene som tegnes, er basert p� koden til "dhpoware". Det st�r en Copyright i toppen av Camera klassen som nevner dette. Jeg tok utgangspunkt i hans/hennes kode da jeg begynte, for � f� en god start. Det jeg n� bruker av dhpoware sin kode gir meg first-person camera funksjonaliteten, samt shaderen og tegningen av veggene i spillet. Shaderen gir meg parallax mapping, noe som var fin "eye-candy" da jeg startet opp.

Mapsene er laget ved � angi koordinater for alle objektene for h�nd. Jeg visste ikke hvor langt jeg skulle ta spillet da jeg begynte, og bestemte meg for � possisjonere objekter manuelt. Etterp�klokskap sier meg at det var en forferdelig d�rlig ide, da jeg har brukt MYE tid p� � tweake p� possisjonene til objeter o.l, men n� ble det da en gang s�nn. Selve vegg-possisjonene har jeg f�tt vet � tegne mapsene mine med svg-edit p� nett (et svg tegneprogram online av google), kopiert svg koden inn i excel, formatert ut x og y koordinatene og brukt disse som x og z koordinater i spillet mitt. Jeg sjekker kun kollisjon i x og z planet, da alt foreg�r p� 0 i Y planet. Dermed var det lett � plassere vegger, da de alltid vil ha possisjonen (x, 0, y) til (x2, wallHeight, y2).

Jeg anbefaler p� det aller sterkeste at du bruker headphones, eventuelt gode surround h�ytalere n�r du spiller spillet. 3D lyd-effekter er en vikgig del av spillet, og fungerer b�de som hjelp og � gi en riktig stemning. 

P� start-menyen er det ogs� mulighet for � velge mellom easy, normal og hard difficulity. Eneste forskjellen mellom disse er at det er fjernet og lagt til en del vegger i labyrintene som gj�r det enklere/vanskeligere � finne frem. Hard er mer eller mindre slik spillet var bygget opp orginalt, men jeg fant ut det var alt for vanskelig og bestemte meg for � fjerne en del vegger. Istedenfor � slette dem fullstendig, la jeg til et simpelt difficulity system, slik at man kan velge om man vil spille med alle veggene eller ikke.

Jeg vil absolutt anbefale � spille p� Easy, da jeg har en mistanke om at medium/hard kun blir kjedelig da det er for mye labyrint ;) H�per ogs� du tar deg tid til � spille igjenom hele spillet, da jeg mener spillet som en helhet er det som er noe spesielt med det. De som har testet det for meg har brukt mellom 10 og 30 min p� � fullf�re det.


Prosjektet ligger ogs� p� github : https://github.com/skakri09/LabyrinthExplorer 
Der er det litt utviklings stuff som ikke er lagt med i innleveringen ogs�, som noen flotte excel ark med wall-positions o.l (ikke egentlig veldig flotte).