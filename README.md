# CLI-Based Password Manager - `psmng`

> **Master key** is stored in `data/` folder for easier debugging for now.

---

## Commands
| Command                | Description                                       |
|------------------------|---------------------------------------------------|
| `new [login] [password]` | Create/Update new login with password           |
| `get [login]`            | Outputs selected login with decrypted password  |
| `del [login]`            | Deletes selected login entry                    |
| `list`                   | List all password entries (encrypted)           |
| `data-path `             | Shows the path to the data.json file            |

---

## Usage
> **psmng.exe** has to be added to system variables
>
> `data/` is created upon running a command
```bash
psmng <command> [arguments]
```
