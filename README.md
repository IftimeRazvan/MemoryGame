🧠 MemoryGame
Aplicație desktop WPF pentru un MemoryGame, în care jucătorii trebuie să asocieze perechi de imagini înainte ca timpul să expire.

📋 Descriere
MemoryGame este o aplicație construită pe .NET 9 și WPF, structurată pe pattern-ul MVVM. Utilizatorii își aleg un profil local, configurează tabla de joc după preferințe și pot relua oricând o sesiune salvată automat. Statisticile și progresul fiecărui jucător sunt păstrate în fișiere locale pentru a urmări evoluția în timp.

🚀 Caracteristici principale
- Interfață modernă WPF cu animații de flip pentru cărți și layout adaptabil în funcție de dimensiunea tablei.
- Gestionarea profilurilor: creare, selectare și ștergere de jucători cu avatar dedicat.
- Configurarea jocului: alegere rapidă a dimensiunii grilei și a categoriei de imagini (Mașini, Animale, Legume etc.).
- Gameplay cronometrat: potrivirea perechilor înainte de expirarea timer-ului, cu logica de validare pentru flip-uri și potriviri.
- Salvare și reluare automată: la închiderea ferestrei jocului, starea curentă se salvează într-un fișier JSON specific jucătorului.
- Statistică pe termen lung: număr de jocuri jucate/câștigate pentru fiecare profil, afișată în ecranul dedicat.


🎮 Fluxuri principale în aplicație
Experiența jucătorilor
- Autentificare locală: ecranul de început listează toți jucătorii din `Data/Users.txt`; se pot crea profiluri noi cu avatar la alegere.
- Configurare joc: din meniul principal se alege categoria de imagini, dimensiunea grilei și se poate încărca un joc salvat anterior.
- Sesiunea de joc: jucătorul întoarce cărți perechi; potrivirile corecte rămân vizibile, iar cele greșite se întorc automat.
- Finalizarea rundei: când toate perechile sunt rezolvate sau timer-ul expiră, statistica se actualizează și poate fi consultată în panoul de scoruri.

Panou statistică
- Vizualizare rezultate: ecranul Stats afișează pentru fiecare jucător numărul de jocuri jucate și câte au fost câștigate.
- Reluare progres: din meniul principal se poate relua cea mai recentă salvare pentru profilul selectat.


🗃️ Persistența datelor
- `Data/Users.txt` stochează profilurile sub forma `nume,caleAvatar`. Modificarea fișierului actualizează imediat lista din ecranul de logare.
- `Data/Stats5.txt` păstrează statistica globală cu formatul `nume-jocuri-jucate-jocuri-câștigate`.
- Salvările active se serializează în fișiere `<nume>.json` în directorul de lucru și sunt încărcate când utilizatorul alege opțiunea „Open Game”.
