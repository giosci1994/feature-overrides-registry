# Security & Safety notes

- Modificare il registro può rendere il sistema instabile.
- Prima di abilitare gli override:
  - crea un **Restore Point**
  - annota lo stato attuale dei driver del disco in **Device Manager**
- Se noti problemi: usa `Disable` e riavvia.

## Safe Mode (advanced)
Alcuni utenti riportano che dopo il cambio dello stack storage, l’avvio in Safe Mode potrebbe richiedere
la presenza del class-id "Storage Disks" sotto `SafeBoot\Network` e `SafeBoot\Minimal`.
Questa repo offre un comando opzionale `fix-safeboot`, ma non è una modifica ufficiale.
