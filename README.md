# Sprint 0 - Game Design Document : Tower Defense  
**Naam:** Arthur  
**Klas:** GD1B 
**Datum:** 08/09/2025  

---

## 1. Titel en elevator pitch  
**Titel:** Dragon defense

**Elevator pitch (maximaal twee zinnen):**  
My game is a tower defense where the player can build different types of towers. Every 10 waves the towers must be swapped, forcing the player to adapt and adding variety and strategy to the gameplay.  

---

## 2. Wat maakt jouw tower defense uniek  
Most tower defense games let players pick a "meta" and stick with it. In my game, every 10 waves the towers **must** be changed, preventing repetitive strategies and keeping the player engaged with new combinations.  

---

## 3. Schets van je level en UI  
Hier voeg je schetsen of mockups in van je level, UI en HUD.  

![Level Sketch](Images_for_README/Image2.png)  

---

## 4. Torens  
**Basis torens:**  
- **Fast Tower** – Low damage, very high fire rate.  
- **Slow Tower** – High damage, slow fire rate.  
- **Long Range Tower** – Attacks from far away, but weaker shots.  
- **Short Range Tower** – Strong damage but limited range.  
- **Freeze Tower** – Slows enemies temporarily.  
- **Flame Tower** – Damage over time effect.  

**Eventuele extra torens:**  
- To be determined (ideas can be added later).  

📌 Alle eigenschappen zijn uitgewerkt in Trello.  

---

## 5. Vijanden  
**Basis vijanden:**  
- **Normal** – Balanced stats.  
- **Fast** – Weak but very quick.  
- **Heavy** – Slow, tanky, resistant to some effects.  
- **Support** – Buffs/heals other enemies.  
- **Invisible** – Hard to detect, requires special towers to reveal.  

📌 Alle eigenschappen zijn uitgewerkt in Trello.  

---

## 6. Gameplay loop  
- Player builds towers.  
- Enemies spawn in waves.  
- Towers attack enemies automatically.  
- Player earns currency from defeated enemies.  
- Currency is used to place/upgrade towers.  
- Every 10 waves: **mandatory tower swap** (player chooses new loadout).  
- Survive as long as possible.  

![Gameplay Loop Sketch](Images_for_README/Image1.png)  

---

## 7. Progressie  
- Each wave spawns more enemies.  
- Enemies gradually become stronger, faster, and sometimes gain resistances.  
- Boss-like enemies appear at wave milestones (10, 20, 30...).  
- Forced tower swaps push the player to adapt new strategies.  

---

## 8. Risico’s en oplossingen volgens PIO  

- **Probleem 1:** Balancing the towers and enemies.  
  - **Impact:** Game might feel too easy or too hard.  
  - **Oplossing:** Playtesting and adjusting stats regularly.  

- **Probleem 2:** Forced tower swaps may frustrate players.  
  - **Impact:** Players feel they have no control.  
  - **Oplossing:** Give a choice of 2–3 towers each swap so they feel agency.  

- **Probleem 3:** Invisible enemies could be unfair.  
  - **Impact:** Players might feel punished without warning.  
  - **Oplossing:** Introduce tutorials/hints before invisible enemies appear.  

  
---

## 9. Planning per sprint en mechanics (Trello-sync)

> Copy this into your README. Each item is a GitHub checkbox you can tick off.

### Sprint 1 — Core loop online
- [ ] Pathing system (waypoints)
- [ ] WaveManager (start/stop waves, per-enemy delay)
- [ ] Placeable spots (grid/slots)
- [ ] Basic Tower (shoot timer, range check)
- [ ] **Projectile system** (move → hit → apply damage)
- [ ] **Collision & damage** (projectile ↔ enemy)
- [ ] **UI basic** (money + lives display)

### Sprint 2 — First playable
- [ ] Normal enemy (baseline stats)
- [ ] Spotting/targeting system (closest/first-in-path)
- [ ] HP system (enemy health bars optional)
- [ ] **Enemy death & reward** (money on kill)
- [ ] **Goal/life loss** (despawn on goal, -1 life)
- [ ] **Build/sell flow** (place, cancel, sell refund)

### Sprint 3 — Variety & economy
- [ ] Economy system (costs, income, balance vars)
- [ ] Light enemy (fast/low HP)
- [ ] Heavy enemy (slow/high HP, maybe slow-resist)
- [ ] Invisible enemy (requires reveal)
- [ ] Support enemy (buff/heal nearby)
- [ ] Low-range tower (high DPS, short range)
- [ ] **Tower upgrades** (range/damage/attack speed)

### Sprint 4 — Unique feature & depth
- [ ] **Tower swap mechanic** (mandatory swap every 10 waves)
- [ ] Freeze tower (slow effect, durations/stacks)
- [ ] Flame tower (DoT over time)
- [ ] **Reveal mechanic** (counter to Invisible)
- [ ] **UI upgrade menu** (inspect/upgrade/sell)
- [ ] **Enemy resistances** (slow/DoT/armor flags)

### Sprint 5 — Boss & polish
- [ ] Boss enemy (wave milestone, phases or abilities)
- [ ] Sound effects (shoot,Music)


---

## 10. Inspiratie  
De game **Bloons Tower Defense (BTD6)** inspireert mij, vooral hoe de game veel verschillende torens en strategieën aanbiedt.  
Wat ik meeneem: variatie in torens en vijanden, duidelijke progressie en feedback.  
Wat ik vermijd: te veel grind of repetitieve strategieën — mijn unieke feature dwingt juist tot afwisseling.  

---

## 11. Technisch ontwerp mini  

### 11.1 Vijandbeweging over het pad  
- **Keuze:** Vijanden volgen een reeks waypoints tot aan de goal.  
- **Risico:** Vijanden lopen een waypoint voorbij of blijven hangen.  
- **Oplossing:** Check afstand tot waypoint; als dichtbij genoeg → ga naar de volgende. Bij goal: verwijder vijand en verlaag levens met 1.  
- **Acceptatie:** 10 vijanden bewegen van start naar goal zonder vast te lopen en verbruiken elk één leven.  

### 11.2 Doel kiezen en schieten  
- **Keuze:** Torens zoeken automatisch het dichtstbijzijnde vijand binnen hun bereik en schieten projectielen.  
- **Risico:** Torens kiezen geen doel of blijven op een dood vijand schieten.  
- **Oplossing:** Regelmatige check op actieve vijanden in range en switch target als nodig.  
- **Acceptatie:** Elke toren valt altijd een geldig vijand aan zolang die in range is.  

### 11.3 Waves en spawnen  
- **Keuze:** Vijanden spawnen in golven met een korte interval per vijand. Elke wave wordt moeilijker.  
- **Risico:** Te veel vijanden tegelijk → lag of onbalans.  
- **Oplossing:** Max aantal vijanden per wave en wachttijd tussen spawns instellen.  
- **Acceptatie:** Elke wave spawnt vijanden met toenemende moeilijkheid zonder technische problemen.  

### 11.4 Economie en levens  
- **Keuze:** Speler krijgt geld door vijanden te verslaan. Elke vijand bij de goal kost 1 leven.  
- **Risico:** Te weinig of te veel geld → slechte balans.  
- **Oplossing:** Geldwaarden en kosten instelbaar via variabelen zodat balans snel kan worden aangepast.  
- **Acceptatie:** Speler kan torens plaatsen/upgraden met verdiend geld en verliest levens als vijanden de goal bereiken.  

### 11.5 UI basis  
- **Keuze:** Simpele UI met geld, levens en wave-informatie.  
- **Risico:** UI is onduidelijk of past niet goed bij schermresolutie.  
- **Oplossing:** UI bouwen met canvas en anchors zodat het schaalt op verschillende schermen.  
- **Acceptatie:** Speler ziet altijd zijn geld, levens en huidige wave duidelijk in beeld.  


### 12???
[Trello](https://trello.com/invite/b/68be86f11b0fd1599436af44/ATTI4fae26223c69d7fe3d588c43749edded7E21C96A/towerdefense)