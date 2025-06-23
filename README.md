# CLI-Based Password Manager - `psmng`

> **Master key** & **Master Password** are stored in `data/` folder for easier debugging (temporary).

---

## Commands
### > Login Management
| Command                  | Description                                              |
|--------------------------|----------------------------------------------------------|
| `new [login] [password]` | Create/Update a login entry with the given password      |
| `get [login]`            | Show the password for selected login entry               |
| `del [login]`            | Deletes selected login entry                             |
| `list`                   | List all login entries (encrypted)                       |

### > Group Management
| Command                  | Description                                              |
|--------------------------|----------------------------------------------------------|
| `new-group [group_name]` | Create a new group                                       |
| `get-group [group_name]` | Show all logins within the specified group               |
| `del-group [group_name`  | Deletes selected group                                   |
| `list-group`             | List all existing groups                                 |
| `add-togroup`            | Add a login to the selected group                        |

### > Path Utilities
| Command                  | Description                                              |
|--------------------------|----------------------------------------------------------|
| `get-path`               | Show the program path                                    |

### > Master Password Management
| Command                             | Description                                   |
|-------------------------------------|-----------------------------------------------|
| `set-masterpassword [new_password]` | Work in progress...                           |
| `get-masterpassword`                | Show current master passwor                   |
| `set-timeout [minutes]`             | Work in progress...                           |
| `get-timeout`                       | Get cache timeout time (minutes)              |

### > Options
| Command                  | Description                                              |
|--------------------------|----------------------------------------------------------|
| `-h`                     | Display all commands and their description               |
| `--version`              | Show program's verison                                   |

---

## Usage
> **psmng.exe** has to be added to system variables
>
> `data/` is created upon running a command
```bash
psmng <command> [arguments]
```

---

## Example
### > Input
```bash
1. psmng new login@example.com password123
2. psmng new-group github
3. psmng add-togroup github login@example.com
4. psmng get login@example.com
```

### > Output
> On first use, `psmng` will prompt you to create **master password**
>
> Once entered, master password is **cached** for the next 30 minutes to avoid repeated prompts
```bash
1. Added 'login@example.com' with password 'password123'
2. Group 'github' has been created.
3. Login 'login@example.com' was added to group 'github'.
4. LOGIN:
     login@example.com
   GROUP:
     github
   PASSWORD:
     password123
```

---

> **Note:** This project intended for **personal use** and for my own self-improvement as a beginner developer.
>
> \> Use at your own risk.
