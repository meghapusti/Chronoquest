# ⏳ ChronoQuest: A Journey Through Time

> *"People will forget what they read, but they'll remember what they experienced."*

A fully immersive, standalone VR educational game for **Meta Quest 3** that transports players across three distinct historical eras — a Prehistoric cave, a Medieval village, and a Futuristic city. Built in Unity with C# for a university Extended Reality (XR) course.


---

## 📖 Table of Contents

- [Overview](#-overview)
- [Gameplay](#-gameplay)
- [Historical Eras](#-historical-eras)
- [Core Mechanics](#-core-mechanics)
- [Technical Architecture](#-technical-architecture)
- [Controls](#-controls)
- [Performance Optimisations](#-performance-optimisations)
- [Challenges & Solutions](#-challenges--solutions)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Built With](#-built-with)

---

## 🌍 Overview

Traditional history education is passive — textbooks, lectures, documentaries. ChronoQuest flips that model entirely.

Instead of reading about the past, **you step inside it**. Players explore three historical environments, throw physics-based balls at targets to unlock historical facts, collect artifacts, and carry them between eras as a tangible metaphor for the flow of time. Every mechanic is designed to make learning active, contextual, and memorable.

**Research Question:**
> *How can Extended Reality transform history from something we read about into something we can step inside and experience?*

---

## 🎮 Gameplay

The full experience runs **5–10 minutes** and requires no tutorial. The gameplay loop across every era follows this structure:

```
Spawn in Era
     │
     ▼
Explore the environment freely
     │
     ▼
Find and hit 3 targets (ball throw mechanic)
     │
     ▼
Each hit → Fun Fact revealed + 10 points awarded
     │
     ▼
3rd target hit → Mystery artifact from next era spawns
     │
     ▼
Pick up artifact → store in persistent backpack inventory
     │
     ▼
Interact with teleport button → travel to next era
     │
     ▼
Return artifact to its correct location in new era
     │
     ▼
Repeat for all 3 eras → Final trophy spawns on completion
```

**Maximum score: 90 points** (3 targets × 10 points × 3 eras)

---

## 🏛️ Historical Eras

### 🔥 Prehistoric Era — The Cave

A dimly lit natural cave with fire pits, primitive tools, and raw terrain. Particle systems simulate flickering fire and smoke. Ambient audio uses bass and percussion-driven soundscapes.

<!-- ![Prehistoric Cave Scene](screenshots/cave-scene.png) -->

> *Focus: Early human origins, survival, fire discovery, and basic toolmaking.*

---

### ⚔️ Medieval Era — The Village

A rustic township with timber-frame buildings, market stalls, wells, and period props. Custom skybox and directional daylight lighting. Ambient audio features lutes, strings, and medieval town sounds.

<!-- ![Medieval Village Scene](screenshots/medieval-scene.png) -->

> *Focus: Social structures, craftsmanship, and feudal community organisation.*

---

### 🚀 Futuristic Era — The City

A sleek high-tech cityscape with neon lighting, holographic elements, and metallic/glass shader materials. Dynamic colored accent lighting. Ambient audio features electronic hums and digital soundscapes.

<!-- ![Futuristic City Scene](screenshots/futuristic-scene.png) -->

> *Focus: Technological advancement, innovation, and speculative futures.*

---

## ⚙️ Core Mechanics

### 🎯 Target-Based Learning System
Three targets are placed per era to encourage full spatial exploration. Players spawn and throw a ball using realistic VR physics. Each destroyed target delivers immediate feedback: visual pop, audio cue, and score update.

### 📜 Fun Fact Reveal System
On target destruction, a floating UI panel appears with a historically themed fact contextual to that era. Facts are stored as **ScriptableObjects** and delivered at the exact moment of successful interaction — maximising retention through action-linked discovery.

### 🎒 Artifact Inventory & Cross-Scene Persistence
After the third target, a mystery artifact from the **next era** appears. Players pick it up and store it in a backpack-style inventory that:
- Uses **XR Socket Interactors** for physical pickup
- Persists via **`DontDestroyOnLoad`** across scene loads
- Displays a visual icon to confirm inventory state

### 🔄 Artifact Return Mechanic
In each new era, players must locate the correct spot and return the carried artifact. This restores narrative order between time periods and acts as the progression unlock for the next set of targets.

### 🏆 Completion Trophy
Destroying the final target in the Futuristic era spawns a trophy — a symbolic mission-accomplished reward and closure for the full learning journey.

---

## 🏗️ Technical Architecture

### Scene & State Management
All persistent systems (score, inventory, game state) are implemented as **Singletons** using `DontDestroyOnLoad`, surviving async scene transitions via `SceneManager.LoadSceneAsync()`. Each scene re-initializes its own camera references, UI bindings, and spawn logic independently.

```csharp
// Score persists across all scenes via Singleton
public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance;
    void Awake() {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }
}
```

### Throw Physics System
Ball throwing is driven by **XR controller velocity tracking** at the moment of release:

```csharp
// Velocity captured at release and applied to Rigidbody
Vector3 releaseVelocity = xrController.velocity * forceMultiplier;
ballRigidbody.linearVelocity = releaseVelocity;
```
Force multipliers were fine-tuned through iterative playtesting to feel natural and controllable.

### Target Hit Detection
Targets use `OnCollisionEnter()` checking for the `"Stone"` tag. Additional expanded colliders around each target improve detection reliability without affecting visual fidelity.

```csharp
void OnCollisionEnter(Collision collision) {
    if (collision.gameObject.CompareTag("Stone")) {
        DestroyTarget();
        ScoreManager.Instance.AddScore(10);
        ShowFunFact();
    }
}
```

### Scene Transitions
1. Player activates `XRBaseInteractable` push button
2. Fade-to-black via Unity Canvas Overlay
3. `SceneManager.LoadSceneAsync()` loads next era
4. `SpawnManager` re-positions player at defined spawn point
5. UI, camera, and orientation reset — state is preserved

### Educational Content (ScriptableObjects)
```
FunFactData (ScriptableObject)
├── eraName: string
├── factText: string
└── targetIndex: int
```
Facts are fully decoupled from scene logic — easy to edit, extend, or localise independently.

---

## 🕹️ Controls

| Action | Control |
|---|---|
| Grab / Interact | Grip Button |
| Throw Ball | Trigger Button (hold + release) |
| Move (Smooth Locomotion) | Left Thumbstick |
| Rotate View | Right Thumbstick |
| Activate Teleport Button | Trigger Button |

---

## ⚡ Performance Optimisations

Targeting a stable **72fps** on Meta Quest 3 standalone hardware required careful optimisation across every scene:

| Technique | Purpose |
|---|---|
| **LOD Groups** | Reduces polygon count on distant meshes |
| **Occlusion Culling** | Skips rendering of off-screen objects |
| **Baked Lighting** | Eliminates real-time lighting cost for static elements |
| **Texture Atlasing** | Combines textures to reduce draw calls |
| **Material Batching** | Groups materials to reduce render state changes |
| **Object Pooling** | Reuses ball prefabs and particle effects instead of instantiating new ones |

---

## 🔧 Challenges & Solutions

| Challenge | Solution |
|---|---|
| **Cross-scene state persistence** | DontDestroyOnLoad Singleton architecture for ScoreManager, InventoryManager, GameManager |
| **72fps on standalone VR** | LOD, occlusion culling, baked lighting, texture atlasing, material batching |
| **Throw physics feel** | Captured controller velocity vector at release; iterated force multipliers via playtesting |
| **Inconsistent target hit detection** | Explicit `"Target"` tags + expanded colliders around hit zones |
| **Misaligned imported 3D models** | Sourced models with clean import settings; replaced problematic assets at source |
| **Player disorientation on scene load** | Fixed spawn points per era managed by SpawnManager; automatic camera + orientation reset |

---

## 📁 Project Structure

```
ChronoQuest/
├── Assets/
│   ├── Scenes/
│   │   ├── PrehistoricEra.unity
│   │   ├── MedievalEra.unity
│   │   └── FuturisticEra.unity
│   ├── Scripts/
│   │   ├── Managers/
│   │   │   ├── ScoreManager.cs
│   │   │   ├── InventoryManager.cs
│   │   │   └── GameManager.cs
│   │   ├── Mechanics/
│   │   │   ├── BallThrow.cs
│   │   │   ├── TargetBehaviour.cs
│   │   │   ├── ArtifactPickup.cs
│   │   │   └── ArtifactReturn.cs
│   │   ├── UI/
│   │   │   ├── FunFactPanel.cs
│   │   │   └── ScoreUI.cs
│   │   └── Navigation/
│   │       ├── SceneTransition.cs
│   │       └── SpawnManager.cs
│   ├── ScriptableObjects/
│   │   └── FunFacts/
│   │       ├── PrehistoricFacts.asset
│   │       ├── MedievalFacts.asset
│   │       └── FuturisticFacts.asset
│   ├── Prefabs/
│   ├── Audio/
│   └── Materials/
├── Packages/
└── ProjectSettings/
```

---

## 🚀 Getting Started

### Prerequisites
- Unity **6000.2.7f2** (or compatible LTS version)
- Unity **XR Interaction Toolkit** package
- **Meta Quest Developer Hub** (for device deployment)
- Meta Quest 3 headset with Developer Mode enabled

### Running in the Editor (with Link)
1. Clone this repository
   ```bash
   git clone https://github.com/meghapusti/chronoquest.git
   ```
2. Open the project in Unity 6000.2.7f2
3. Install required packages via **Package Manager**:
   - XR Interaction Toolkit
   - XR Plugin Management
   - Oculus XR Plugin
4. Connect Meta Quest 3 via **Meta Quest Link** or **Air Link**
5. Open the `PrehistoricEra` scene and press **Play**

### Building for Meta Quest 3
1. Go to **File → Build Settings**
2. Switch platform to **Android**
3. Set Texture Compression to **ASTC**
4. Enable **XR Plugin Management → Oculus** under Android settings
5. Click **Build and Run** with headset connected via USB

> ⚠️ Ensure Developer Mode is enabled on your Quest 3 via the Meta Quest mobile app before deploying.

---

## 🛠️ Built With

- **[Unity 6000.2.7f2](https://unity.com/)** — Game engine
- **C#** — All gameplay logic, interaction, and scene management
- **[Unity XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)** — VR interaction framework
- **[Meta Quest 3](https://developer.oculus.com/)** — Target hardware (Snapdragon XR2 Gen 2)
- **Visual Studio 2022** — IDE
- **GitHub** — Version control and collaboration
- **Unity Asset Store** — Base environmental and prop assets

---

## 📄 License

This project was developed for academic purposes as part of a university course. All third-party assets are used under their respective licenses from the Unity Asset Store.

---

*ChronoQuest — because history is better experienced than memorised.*
