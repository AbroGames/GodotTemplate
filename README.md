# GodotTemplate
Godot game template with essential utilities

<img width="501" height="538" alt="image" src="https://github.com/user-attachments/assets/1c76b953-7ca7-4e00-b1f5-10e7f5f338f7" />

<img width="472" height="406" alt="image" src="https://github.com/user-attachments/assets/ad7d14ba-d346-41b0-b2d4-2bf6f39fb954" />

<img width="1248" height="846" alt="image" src="https://github.com/user-attachments/assets/f8ed9786-0ff5-4dba-b6b8-a642f0b49e1b" />

<img width="718" height="375" alt="image" src="https://github.com/user-attachments/assets/fa28baa9-731e-4d22-bb53-085b2c277abd" />

<img width="802" height="779" alt="image" src="https://github.com/user-attachments/assets/93f1fc80-bd9f-405e-bfbd-e07db2847812" />

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
