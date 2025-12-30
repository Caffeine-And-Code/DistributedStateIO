# Game Rules

## Number of Players

To create a lobby and start a match, **exactly 6 players** are required.

A lobby is automatically created when the first player joins.
The match **automatically starts as soon as 6 players have joined the lobby**.

Each game must always have **6 players**.

If a player gets disconnected, a timer is started. When the timer expires, if the player is still offline, they are removed from the match and their territories are released.

The game can operate in **one of two reconnection modes**, which are **decided by the administrator of the game platform**:

- All other players remain active and can continue playing (including attacking the disconnected player’s territories).
- All players are put on hold until the disconnected player reconnects or is removed.

## Setup

At the beginning of the match, each player is assigned a random territory and a unique color.

As a result, the game map will contain **6 owned territories**, while the remaining ones are considered **neutral territories**.

Each state starts with a predefined number of troops.

## State Troop Generation

Each state produces a certain number of troops at fixed time intervals.

States owned by a player generate troops **without any upper limit**, while unoccupied states can hold **up to the number of troops they initially started with**.

If an unoccupied state loses troops (which is considered an occupation cost) but is not conquered, its troop count will regenerate back up to its initial value, and not beyond.

The total number of troops stationed in a state is represented by a number displayed at the center of that state.

## Commands

The following actions can be performed during a match.
To execute any action, the player must first click on a state under their control (**origin state**) and then on a second state (**target state**).

The troops involved in any command are calculated as **all troops available in the origin state minus one**, which always remains stationed in the origin state.
As a result, an attack can only be launched from a state with **at least two troops**.

### Troop Transfer

If the target state is owned by the same player, troops can be transferred as reinforcements.
The troop count of the origin state is reduced accordingly, while the target state’s troop count is increased by the same amount.

### Attack Behaviors

If the target state is owned by another player or is a neutral territory, an attack is initiated.

* **Neutral territory**:
  If the number of troops on the neutral territory is reduced to a value less than or equal to zero, the territory is conquered by the attacker, and the number of troops stationed on it becomes equal to the absolute value of the difference between the attacking troops and the troops originally present in the territory.

* **Enemy territory**:
  One of the following situations occurs.

#### Direct Attack

The attack is not intercepted by the defending player.
The number of troops on the defending state is reduced by the number of attacking troops.

- If the defending state remains with a troop count greater than or equal to zero, it stays under the original owner’s control.
- If the result is negative, the state is conquered by the attacker, and the number of troops stationed on it equals the absolute value of the difference between attacking and defending troops.

#### Bilateral Attack

The attack is intercepted by the target state: the defending player simultaneously launches an attack toward the origin state, and the two armies meet halfway.

The number of troops from the second attack (in chronological order) is subtracted from the first one:

- If the result is positive, the first attack proceeds as a **direct attack**.
- If the result is negative, the second attack proceeds as a **direct attack**.
- If the result is zero, both attacking forces are completely eliminated.

## Spectators

Spectators can watch the match but do not control any state.

Each spectator has access to a set of reactions that can be sent and displayed to all match participants, including other spectators.
Between one reaction and the next, spectators must wait for a cooldown period.
