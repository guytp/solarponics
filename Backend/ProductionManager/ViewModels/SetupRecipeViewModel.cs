﻿using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupRecipeViewModel : ViewModelBase, ISetupRecipeViewModel
    {
        private readonly IRecipeApiClient recipeApiClient;
        private readonly IDialogBox dialogBox;

        public SetupRecipeViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IRecipeApiClient recipeApiClient, IDialogBox dialogBox)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.recipeApiClient = recipeApiClient;
            this.dialogBox = dialogBox;
            this.AddCommand = new RelayCommand(_ => this.Add());
            this.DeleteCommand = new RelayCommand(_ => this.Delete());
            this.IsUiEnabled = true;
            Types = new RecipeType?[]
            {
                null,
                RecipeType.Agar,
                RecipeType.FruitingBlock,
                RecipeType.GrainSpawn,
                RecipeType.LiquidSpawn
            };
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public bool IsAddEnabled => !string.IsNullOrWhiteSpace(this.NewName) && !string.IsNullOrWhiteSpace(this.NewText) && NewType.HasValue;
        public bool IsDeleteEnabled => this.SelectedRecipe != null;
        public bool IsUiEnabled { get; private set; }
        public Recipe[] Recipes { get; private set; }
        public Recipe SelectedRecipe { get; set; }
        public RecipeType?[] Types { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public string NewName { get; set; }
        public string NewText { get; set; }
        public RecipeType? NewType { get; set; }

        public override async Task OnShow()
        {
            this.Recipes = await this.recipeApiClient.Get();
            this.SelectedRecipe = null;
            this.NewName = null;
            this.NewText = null;
            this.NewType = null;
        }
        
        private async void Add()
        {
            if (!this.IsUiEnabled || !this.IsAddEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;
                var recipe = await this.recipeApiClient.Add(new Recipe
                {
                    Name = NewName,
                    Text = NewText,
                    Type = NewType.Value
                });
                var recipes = new List<Recipe>();
                if (this.Recipes != null && this.Recipes.Length > 0)
                {
                    recipes.AddRange(this.Recipes);
                }
                recipes.Add(recipe);
                this.Recipes = recipes.OrderBy(s => s.Name).ToArray();
                this.NewName = null;
                this.NewText = null;
                this.NewType = null;
                this.dialogBox.Show("Recipe added.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to add recipe.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private async void Delete()
        {
            if (!this.IsUiEnabled || !this.IsDeleteEnabled)
                return;

            if (this.SelectedRecipe == null)
                return;

            if (!this.dialogBox.Show("Do you want to delete " + this.SelectedRecipe.Name + "?", buttons: Enums.DialogBoxButtons.YesNo))
            {
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                await this.recipeApiClient.Delete(this.SelectedRecipe.Id);
                this.Recipes = this.Recipes.Where(s => s != this.SelectedRecipe).ToArray();
                this.SelectedRecipe = null;
                this.dialogBox.Show("Recipe deleted.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to delete recipe.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}