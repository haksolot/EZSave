# EZSave App
## Version 2 - GUI (Graphical User Interface)

## Features  

- Full file backup
- Differential backup (only modified or new files)  
- Logging and backup status management  
- WPF interface (Graphical User Interface)
- Changing language (french and english)
- Ciphering
- Illimited backups
- Changing logs types  

You can also have access to unit tests !

---

## Installation & Execution  

### 1. Prerequisites  
- .NET 8  


### 2. Clone the project  
```bash
git clone https://github.com/haksolot/EZSave
cd EZSave
```

### 3. Execute EZSave.TUI

### 4. Find logs, status updates and conf files
Logs, status information and conf file are stored in the project's temporary files : *EZSave\EZSave.TUI\bin\Debug\net8.0*

## Using the app  

### 1. The interface (French version by default) 

At the top:

- "FR" button: Switch language to French
- "EN" button: Switch language to English
- "Ajouter" button: Add a job
- "Exécuter tout" button: Execute all jobs
- "Mode configuration" button: Enter configuration mode
- "Exécuter" button: Execute selected jobs

At the bottom left:

- Tab containing all added jobs

At the bottom right:

- Tab containing selected jobs ready to be executed (more information below)

At the bottom center:

- ▶︎ button: Add a job to the selection
- ◀︎ button: Remove a job from the selection

### 2. Adding a job

- Click the "Ajouter" button
- Enter the name of the job, the source, destination and backup type
- Click the "Validate" button
- Close the window
  => The job is added to the left tab

### 3. Executing ALL jobs

- Add a job (see 2. Add a job)
- Click the "Exécuter tout" button
- Check logs and status of the jobs (4. Find logs, status updates and conf files)
  => All jobs are executed if the inputs are correct
  
### 4. Executing a SELECTION of jobs

The job selection feature allows you to execute a specific group of jobs instead of all jobs.

  - Add a job (see 2. Add a job)
  - Select the job in the left tab that you want to add to the selection
  - Click the ▶︎ button
  - (The job is added to the right tab)
  - Click the "Exécuter" button
  - Check logs and status of the jobs (4. Find logs, status updates and conf files)
    => All selected jobs are executed if the inputs are correct
    
To remove a job from the selection, click the ◀︎ button

### 5. Modifying a job

- Click the "Mode Configuration" button
- Select the job you want to modify in the tab
- Modify the desired fields 
- Click the "Modifier" button
   => The job has been modified

### 6. Deleting a job

- Click the "Mode Configuration" button
- Select the job you want to delete in the tab
- Click the "Supprimer" button
   => The job has been deleted from the left tab

### 7. Modifying configuration (top part of window)

- Click the "Mode Configuration" button
- Modify the desired fields (configuration file path, log file path, log types, status file path)
- Click the "Sauvegarder" button
   => The configuration changes have been saved 

### 8. Changing the language

- To switch to French, click the "FR" button
- To switch to English, click the "EN" button
