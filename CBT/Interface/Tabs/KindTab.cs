// // Here be dragons.

// namespace CBT.Interface.Tabs;

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Numerics;
// using CBT.FlyText.Configuration;
// using CBT.Types;
// using Dalamud.Interface.Utility.Raii;
// using ImGuiNET;

// /// <summary>
// /// KindTab configures settings for FlyTextKinds.
// /// </summary>
// public class KindTab : Tab
// {
//     private static FlyTextKind currentKind = Enum.GetValues<FlyTextKind>().Cast<FlyTextKind>().First();

//     private static Dictionary<FlyTextKind, FlyTextConfiguration> tmpConfig = new Dictionary<FlyTextKind, FlyTextConfiguration>();

//     /// <summary>
//     /// Gets the Name of the Tab.
//     /// </summary>
//     public override string Name => TabKind.Kind.ToString();

//     /// <summary>
//     /// Gets the Kind of the Window drawn within the tab.
//     /// </summary>
//     public override TabKind Kind => TabKind.Kind;

//     private static FlyTextKind CurrentKind
//     {
//         get => currentKind;
//         set => currentKind = value;
//     }

//     private static bool CurrentKindEnabled
//     {
//         get => GetValue(config => config.Enabled, false);
//         set => SetValue((config, val) => config.Enabled = val, value);
//     }

//     private static string CurrentFont
//     {
//         get => GetValue(config => config.Font.Name, string.Empty);
//         set => SetValue((config, val) => config.Font.Name = val, value);
//     }

//     private static Vector4 CurrentFontColor
//     {
//         get => GetValue(config => config.Font.Color, default);
//         set => SetValue((config, val) => config.Font.Color = val, value);
//     }

//     private static float CurrentFontSize
//     {
//         get => GetValue(config => config.Font.Size, 0f);
//         set => SetValue((config, val) => config.Font.Size = val, value);
//     }

//     private static bool CurrentOutlineEnabled
//     {
//         get => GetValue(config => config.Font.Outline.Enabled, false);
//         set => SetValue((config, val) => config.Font.Outline.Enabled = val, value);
//     }

//     private static int CurrentOutlineThickness
//     {
//         get => GetValue(config => config.Font.Outline.Size, 0);
//         set => SetValue((config, val) => config.Font.Outline.Size = val, value);
//     }

//     private static Vector4 CurrentOutlineColor
//     {
//         get => GetValue(config => config.Font.Outline.Color, default);
//         set => SetValue((config, val) => config.Font.Outline.Color = val, value);
//     }

//     /// <inheritdoc/>
//     public override void Draw()
//     {
//         // Artist.DrawTitle("FlyText Kinds Configuration");
//         // Artist.DrawSeperator();

//         // using (Service.Fonts.Push(Defaults.DefaultFontName, 16f))
//         // {
//         //     var textPickerWidth = 250f;
//         //     var numPickerWidth = 50f;

//         //     Artist.DrawSeperator();

//         //     if (DrawKindConfiguration(textPickerWidth))
//         //     {
//         //         Artist.DrawSeperator();

//         //         DrawFontConfiguration(textPickerWidth, numPickerWidth);
//         //     }
//         // }

//         // DrawSaveButton();
//     }

//     /// <inheritdoc/>
//     public override void OnClose()
//     {
//         // tmpConfig.Clear();
//     }

//     private static T GetValue<T>(Func<FlyTextConfiguration, T> selector, T defaultValue)
//         => tmpConfig.TryGetValue(CurrentKind, out var currentConfig) ? selector(currentConfig) : selector(Service.Configuration.FlyTextKinds[CurrentKind]);

//     private static void SetValue<T>(Action<FlyTextConfiguration, T> setter, T value)
//     {
//         if (!tmpConfig.TryGetValue(CurrentKind, out var currentConfig))
//         {
//             currentConfig = new FlyTextConfiguration(Service.Configuration.FlyTextKinds[CurrentKind]);
//             tmpConfig[CurrentKind] = currentConfig;
//         }

//         setter(currentConfig, value);
//     }

//     private static void OnSave()
//     {
//         // tmpConfig.Keys.ToList().ForEach(static kind =>
//         // {
//         //     Service.Configuration.FlyTextKinds[kind] = tmpConfig[kind];
//         //     tmpConfig.Remove(kind);
//         // });
//     }

//     // private static void DrawSaveButton()
//     // {
//     //     var colors = new List<(ImGuiCol Style, Vector4 Color)>
//     // {
//     //     (ImGuiCol.Text, new Vector4(1, 1, 1, 1)),
//     //     (ImGuiCol.Button, new Vector4(206 / 255f, 39 / 255f, 187 / 255f, 1.0f)),
//     //     (ImGuiCol.ButtonHovered, new Vector4(39 / 255f, 187 / 255f, 206 / 255f, 1.0f)),
//     //     (ImGuiCol.ButtonActive, new Vector4(1, 1, 0, 1)),
//     // };

//     //     Artist.ColoredButton("Save##Kind", colors, OnSave);
//     // }

//     // private static bool DrawKindConfiguration(float textPickerWidth)
//     // {
//     //     using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
//     //     {
//     //         ImGui.Text("Kind Configuration");
//     //         ImGui.Spacing();
//     //     }

//     //     {
//     //         using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //         {
//     //             ImGui.Text("Select Kind: ");
//     //         }

//     //         ImGui.SameLine();
//     //         ImGui.SetNextItemWidth(textPickerWidth);
//     //         Artist.DrawSelectPicker("Kind", CurrentKind, Enum.GetValues<FlyTextKind>().Cast<FlyTextKind>().ToList().ToList(), kind => { CurrentKind = kind; });

//     //         ImGui.SameLine();
//     //         Artist.Checkbox("Enabled", CurrentKindEnabled, enabled =>
//     //         {
//     //             CurrentKindEnabled = enabled;
//     //         });
//     //     }

//     //     return CurrentKindEnabled;
//     // }

//     // private static void DrawFontConfiguration(float textPickerWidth, float numPickerWidth)
//     // {
//     //     using (Service.Fonts.Push(Defaults.DefaultFontName, 18f))
//     //     {
//     //         ImGui.Text("Font Configuration");
//     //         ImGui.Spacing();
//     //     }

//     //     {
//     //         using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //         {
//     //             ImGui.Text("Select Font: ");
//     //         }

//     //         ImGui.SameLine();
//     //         ImGui.SetNextItemWidth(textPickerWidth);
//     //         Artist.DrawSelectPicker("Font", CurrentFont, [.. PluginConfiguration.Fonts.Keys], name =>
//     //         {
//     //             CurrentFont = name;
//     //         });
//     //         ImGui.SameLine();
//     //         ImGui.SetNextItemWidth(numPickerWidth);
//     //         Artist.DrawSelectPicker("FontSize", CurrentFontSize, PluginConfiguration.Fonts[CurrentFont], size =>
//     //         {
//     //             CurrentFontSize = size;
//     //         });
//     //     }

//     //     using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //     {
//     //         ImGui.Text("Select Font Color: ");
//     //     }

//     //     ImGui.SameLine();
//     //     ImGui.SetNextItemWidth(textPickerWidth);
//     //     Artist.DrawColorPicker("Color##Font", CurrentFontColor, color =>
//     //     {
//     //         CurrentFontColor = color;
//     //     });

//     //     using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //     {
//     //         ImGui.Text("Select Font Outline: ");
//     //     }

//     //     ImGui.SameLine();
//     //     Artist.Checkbox("Outline", CurrentOutlineEnabled, enabled =>
//     //     {
//     //         CurrentOutlineEnabled = enabled;
//     //     });

//     //     if (CurrentOutlineEnabled)
//     //     {
//     //         using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //         {
//     //             ImGui.Text("Select Outline Thickness: ");
//     //         }

//     //         ImGui.SameLine();
//     //         ImGui.SetNextItemWidth(numPickerWidth * 2);
//     //         var userInput = CurrentOutlineThickness;
//     //         if (ImGui.InputInt("##Outline Thickness", ref userInput, 1, 2))
//     //         {
//     //             userInput = Math.Clamp(userInput, 1, 5);

//     //             CurrentOutlineThickness = userInput;
//     //         }

//     //         using (ImRaii.PushColor(ImGuiCol.Text, ImGui.GetColorU32(new Vector4(1, 1, 0, 1))))
//     //         {
//     //             ImGui.Text("Select Outline Color: ");
//     //         }

//     //         ImGui.SameLine();
//     //         ImGui.SetNextItemWidth(textPickerWidth);
//     //         Artist.DrawColorPicker("Color##Outline", CurrentOutlineColor, color =>
//     //         {
//     //             CurrentOutlineColor = color;
//     //         });
//     //     }
//     // }
// }