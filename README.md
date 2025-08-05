# Godot Template

Гайд по созданию и настройке нового проекта в Godot.

### Предварительная настройка Godot


1. Чтобы в Godot настроить интеграцию с Rider, необходимо зайти в Editor -> Editor Settings -> Dotnet -> Editor. В списке External Editor выбрать JetBrains Rider и очистить значение Custom Exec Path Args.  
2. [Опционально] Чтобы иметь возможность при экспорте изменять свойства exe файла, необходимо указать путь до [rcedit](https://github.com/electron/rcedit/releases) в Editor -> Editor Settings -> Export -> Windows -> rcedit.

### Создание проектов в Godot и Rider 

Создаем новый проект. Необходимо в Project Path руками указать путь, чтобы сохранить регистр.  
<img width="400" height="429" alt="image" src="https://github.com/user-attachments/assets/1c76b953-7ca7-4e00-b1f5-10e7f5f338f7" />

В редакторе создаем в любом месте C# скрипт и открываем его, чтобы создался проект в Rider и Solution для C#.  
<img width="400" height="344" alt="image" src="https://github.com/user-attachments/assets/ad7d14ba-d346-41b0-b2d4-2bf6f39fb954" />

### Настройка экспорта

1. Открываем Project -> Export
2. Добавляем нужные экспорты: Windows, Android и т.д.
3. Указываем Export path (пример для Windows): ./bin/win-64/GodotTemplate.exe
4. Если не был настроен rcedit, то отключаем Modify Resources
 
<img width="800" height="542" alt="image" src="https://github.com/user-attachments/assets/f8ed9786-0ff5-4dba-b6b8-a642f0b49e1b" />

Если внизу есть ошибки из-за отсутствующих export template, то нажимаем Manage Export Templates.
1. Нажимаем Go Online
2. Нажимаем Download And Install

<img width="800" height="418" alt="image" src="https://github.com/user-attachments/assets/fa28baa9-731e-4d22-bb53-085b2c277abd" />

Открываем в Rider файл проекта (*.csproj).

<img width="800" height="777" alt="image" src="https://github.com/user-attachments/assets/93f1fc80-bd9f-405e-bfbd-e07db2847812" />

В этот файл внутрь блока Project добавляем блок Target с настройками пре-билда.  
Пример, как должно получиться:
```
<Project Sdk="Godot.NET.Sdk/4.4.1">
  ...
  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <ItemGroup>
      <DirectoriesToCreate Include="$(ProjectDir)bin\win-64" />
      <DirectoriesToCreate Include="$(ProjectDir)bin\android" />
      <FilesToCreate Include="$(ProjectDir)bin\.gdignore" />
    </ItemGroup>
    <MakeDir Directories="@(DirectoriesToCreate)" />
    <WriteLinesToFile File="@(FilesToCreate)" Lines="" Overwrite="true" />
  </Target>
  ...
</Project>
```

### Копирование файлов
Создать папки Scripts, Scenes, Assets

Заменить файлы из GodotTemplate:  
.editorconfig  
.gitignore  

Перенести icon.svg в Assets/Textures/icon.svg

Если был перенос исходников, то необходимо во всех исходниках переименовать GodotTemplate в название проекта.

### Настройка проекта в Godot

Project -> Project Settings -> Application -> Run -> Main Scene = Scenes/Root/Root.tscn  
Project -> Project Settings -> Application -> Config -> Icon = res://Assets/Textures/icon.svg

