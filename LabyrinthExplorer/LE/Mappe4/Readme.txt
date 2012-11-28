Spillet er en 3d labyrint explorer med en slags historie bak. Man beveger seg rundt med wasd keys og styrer retningen med musa på "first person" vis. Målet med spillet er explore seg frem og å løse gåter/puzzles underveis, kan ikke si for mye mer uten å spoile "historien" ;)

3D modellene er hentet fra forskjellige kilder, samt jeg har laget noe selv (alt som ser forferdelig ut altså). Mange modeller og lydfiler har copyrights, er lagt ved en note om det i content folderen.

Jeg må nevne at jeg i denne innleveringen har brydd meg veldig mye mindre om kodekvalitet enn jeg vanligvis gjør, for å få tid til å få med nok content og ha et kult gameplay. Dette er slik jeg har skjønt det i hennhold til slik vi skal gjøre det i dette faget, men jeg nevner det likevel da jeg er klar over at kodekvaliteten langt i fra er på topp.

Selve grunn-kontroller mekanismen i spillet (kameraet), samt veggene som tegnes, er basert på koden til "dhpoware". Det står en Copyright i toppen av Camera klassen som nevner dette. Jeg tok utgangspunkt i hans/hennes kode da jeg begynte, for å få en god start. Det jeg nå bruker av dhpoware sin kode gir meg first-person camera funksjonaliteten, samt shaderen og tegningen av veggene i spillet. Shaderen gir meg parallax mapping, noe som var fin "eye-candy" da jeg startet opp.

Mapsene er laget ved å angi koordinater for alle objektene for hånd. Jeg visste ikke hvor langt jeg skulle ta spillet da jeg begynte, og bestemte meg for å possisjonere objekter manuelt. Etterpåklokskap sier meg at det var en forferdelig dårlig ide, da jeg har brukt MYE tid på å tweake på possisjonene til objeter o.l, men nå ble det da en gang sånn. Selve vegg-possisjonene har jeg fått vet å tegne mapsene mine med svg-edit på nett (et svg tegneprogram online av google), kopiert svg koden inn i excel, formatert ut x og y koordinatene og brukt disse som x og z koordinater i spillet mitt. Jeg sjekker kun kollisjon i x og z planet, da alt foregår på 0 i Y planet. Dermed var det lett å plassere vegger, da de alltid vil ha possisjonen (x, 0, y) til (x2, wallHeight, y2).

Jeg anbefaler på det aller sterkeste at du bruker headphones, eventuelt gode surround høytalere når du spiller spillet. 3D lyd-effekter er en vikgig del av spillet, og fungerer både som hjelp og å gi en riktig stemning. 

På start-menyen er det også mulighet for å velge mellom easy, normal og hard difficulity. Eneste forskjellen mellom disse er at det er fjernet og lagt til en del vegger i labyrintene som gjør det enklere/vanskeligere å finne frem. Hard er mer eller mindre slik spillet var bygget opp orginalt, men jeg fant ut det var alt for vanskelig og bestemte meg for å fjerne en del vegger. Istedenfor å slette dem fullstendig, la jeg til et simpelt difficulity system, slik at man kan velge om man vil spille med alle veggene eller ikke.

Jeg vil absolutt anbefale å spille på Easy, da jeg har en mistanke om at medium/hard kun blir kjedelig da det er for mye labyrint ;) Håper også du tar deg tid til å spille igjenom hele spillet, da jeg mener spillet som en helhet er det som er noe spesielt med det. De som har testet det for meg har brukt mellom 10 og 30 min på å fullføre det.


Prosjektet ligger også på github : https://github.com/skakri09/LabyrinthExplorer 
Der er det litt utviklings stuff som ikke er lagt med i innleveringen også, som noen flotte excel ark med wall-positions o.l (ikke egentlig veldig flotte).