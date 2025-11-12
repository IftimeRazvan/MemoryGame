# 🧠 MemoryGame

O aplicație desktop WPF pentru un **Memory Game**, în care jucătorii trebuie să asocieze perechi de imagini înainte ca timpul să expire.

## 📋 Descriere
**MemoryGame** este o aplicație construită pe **.NET 9** și **WPF**, structurată pe pattern-ul **MVVM**.  
Utilizatorii își aleg un profil local, configurează tabla de joc după preferințe și pot relua oricând o sesiune salvată automat. Statisticile și progresul fiecărui jucător sunt păstrate în fișiere locale, pentru a urmări evoluția în timp.

## 🚀 Caracteristici principale
- **Interfață modernă WPF:** animații fluide pentru flip-ul cărților și layout adaptabil în funcție de dimensiunea tablei.  
- **Gestionarea profilurilor:** creare, selectare și ștergere de jucători, fiecare cu avatar dedicat.  
- **Configurarea jocului:** alegere rapidă a dimensiunii grilei și a temei de imagini (Mașini, Animale, Legume etc.).  
- **Gameplay cronometrat:** potrivirea perechilor înainte de expirarea timer-ului, cu logică de validare pentru flip-uri și potriviri.  
- **Salvare și reluare automată:** la închiderea jocului, starea curentă se serializează într-un fișier JSON asociat jucătorului.  
- **Statistică pe termen lung:** numărul total de jocuri jucate și câștigate pentru fiecare profil, afișate într-un panou dedicat.

## 🎮 Fluxuri principale în aplicație

### Experiența jucătorilor
- **Autentificare locală:** ecranul de start afișează lista jucătorilor existenți din `Data/Users.txt`; se pot crea profiluri noi cu avatar personalizat.  
- **Configurarea jocului:** din meniul principal, utilizatorul selectează tema, dimensiunea grilei și poate încărca un joc salvat anterior.  
- **Sesiunea de joc:** jucătorul întoarce câte două cărți; potrivirile corecte rămân vizibile, cele greșite se închid automat.  
- **Finalizarea rundei:** la completarea tuturor perechilor sau la expirarea timpului, se actualizează statisticile și se oferă opțiunea de rejucare.

### Panoul de statistică
- **Vizualizare rezultate:** ecranul *Stats* afișează pentru fiecare profil numărul de jocuri jucate și câte au fost câștigate.  
- **Reluare progres:** din meniul principal se poate relua ultima salvare pentru jucătorul selectat.

## 🗃️ Persistența datelor
- **`Data/Users.txt`** — stochează profilurile sub forma `nume, caleAvatar`; modificarea fișierului actualizează imediat lista de logare.  
- **`Data/Stats5.txt`** — păstrează statisticile globale în format `nume-jocuri-jucate-jocuri-câștigate`.  
- **Fișiere `<nume>.json`** — salvează automat starea curentă a jocului pentru fiecare jucător, permițând reluarea rapidă.

