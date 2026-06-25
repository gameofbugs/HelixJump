# 🌀 Helix Jump — Can You Score 200?

> **Rotate the helix, dodge the danger platforms, survive the tower. Nobody has scored 200 yet. Think you can?**

![Platform](https://img.shields.io/badge/Platform-Android-3DDC84?style=flat&logo=android&logoColor=white)
![Engine](https://img.shields.io/badge/Engine-Unity-000000?style=flat&logo=unity&logoColor=white)
![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat&logo=csharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-Shipped-brightgreen?style=flat)

---

## 🎮 Play It

[![Play on itch.io](https://img.shields.io/badge/Play_on-itch.io-FA5C5C?style=flat&logo=itch.io&logoColor=white)](https://gameofbugsofficial.itch.io/helixjump)

---

## 📖 About

Helix Jump is a fast-paced reflex challenge where you guide a bouncing ball down a twisting tower. Rotate the helix to avoid danger platforms and push your reflexes to the limit.

It might look simple at first — but don't be fooled. The deeper you go, the trickier it gets. I've shared this with many friends and **nobody has ever scored 200 or more.** Think you can break that record?

---

## 🔹 Features

- **🌀 Rotating Helix Gameplay** — Rotate platforms horizontally to guide your ball safely down the tower
- **⚠️ Dynamic Difficulty** — Starts with 1 danger platform per level, scales up to 4 as your score climbs
- **🔥 Hidden Mechanic** — Clear multiple helixes in one drop and the next platforms go partially invisible — a sneaky extra test for sharp reflexes
- **🏆 Leaderboard** — Track your personal best and challenge friends to beat your high score
- **⚡ Speed Escalation** — Increasing speed over time keeps every run intense

---

## 🕹️ How to Play

- Rotate the helix to guide your ball safely
- Avoid landing on danger platforms (red sections)
- Clear as many helixes as possible for a high score
- The further you go, the faster it gets

---

## 🔧 What I Built

- **Dynamic Danger Platform System** — danger platform count scales with score (1 → 4), calculated per helix generation so difficulty curves smoothly without manual level design
- **Invisible Platform Mechanic** — platforms partially fade out after a combo drop, implemented via material alpha lerp triggered by consecutive clear detection
- **Score-Based Speed Escalation** — ball fall speed and helix rotation sensitivity increase at score thresholds, tuned to stay fair but punishing at high scores
- **Leaderboard & Persistent Best Score** — personal best saved with PlayerPrefs, displayed on death screen with comparison to current run
- **Combo Detection** — tracks consecutive helix clears in a single drop to trigger hidden difficulty and bonus scoring

---

## 🧠 What I Learned

- Designing **emergent difficulty** — scaling danger organically from game state rather than hardcoded level data
- Implementing a **hidden mechanic** that rewards skilled players without punishing beginners — the invisible platforms only appear after a combo, so casual players never see them
- Tuning **feel vs fairness** — getting speed escalation to feel intense without becoming unfair required careful threshold testing
- Using **material property blocks** in Unity to lerp platform transparency at runtime without creating new material instances

---

## ⚙️ Tech

| Tool | Use |
|------|-----|
| Unity | Game engine |
| C# | All game logic |
| PlayerPrefs | Persistent high score |
| Rigidbody3D | Ball physics |
| Material Property Blocks | Invisible platform fade effect |

---

## 🚀 Run Locally

1. Clone: `git clone https://github.com/gameofbugs/HelixJump`
2. Open in Unity Hub
3. Open the main scene and press Play

---

## 👤 About

Built solo by **Manoj S** — one of 7 shipped Android Unity games.
More projects: [gameofbugsofficial.itch.io](https://gameofbugsofficial.itch.io)
