﻿namespace formulate.app.Trees.Helpers
{

    // Namespaces.
    using core.Extensions;
    using Entities;
    using Folders;
    using Forms;
    using formulate.app.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Formatting;
    using Umbraco.Web.Models.Trees;
    using Umbraco.Web.Trees;
    using FormConstants = formulate.app.Constants.Trees.Forms;


    /// <summary>
    /// Helps with forms in the Formulate tree.
    /// </summary>
    internal class FormHelper
    {

        #region Properties

        /// <summary>
        /// The tree controller.
        /// </summary>
        private TreeController Tree { get; set; }


        /// <summary>
        /// The folder helper.
        /// </summary>
        private FolderHelper Helper { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="tree">
        /// The tree controller.
        /// </param>
        /// <param name="helper">
        /// The folder helper.
        /// </param>
        public FormHelper(TreeController tree, FolderHelper helper)
        {
            Tree = tree;
            Helper = helper;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Adds the specified form entities (form or folder) to
        /// the tree.
        /// </summary>
        /// <param name="nodes">
        /// The collection to add the nodes to.
        /// </param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="entities">
        /// The entities (forms and folders) to add to the tree.
        /// </param>
        public void AddFormChildrenToTree(TreeNodeCollection nodes,
            FormDataCollection queryStrings, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {

                // Folder or form?
                if (entity is Folder)
                {
                    var folder = entity as Folder;
                    Helper.AddFolderToTree(nodes, queryStrings, folder,
                        FormConstants.GroupIcon);
                }
                else if (entity is Form)
                {
                    var form = entity as Form;
                    AddFormToTree(nodes, queryStrings, form);
                }

            }
        }


        /// <summary>
        /// Adds a form node to the tree.
        /// </summary>
        /// <param name="nodes">
        /// The node collection to add the form to.
        /// </param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="form">The form to add.</param>
        public void AddFormToTree(TreeNodeCollection nodes,
            FormDataCollection queryStrings, Form form)
        {
            var formatUrl = "/formulate/formulate/editForm/{0}";
            var formId = GuidHelper.GetString(form.Id);
            var formRoute = string.Format(formatUrl, formId);
            var formName = form.Name.Fallback("Unnamed");
            var parentId = form.Path[form.Path.Length - 2];
            var strParentId = GuidHelper.GetString(parentId);
            var formNode = Tree.CreateTreeNode(formId,
                strParentId, queryStrings,
                formName, FormConstants.ItemIcon, false, formRoute);
            nodes.Add(formNode);
        }


        /// <summary>
        /// Adds the "Delete Form" action to the menu.
        /// </summary>
        /// <param name="menu">
        /// The menu items to add the action to.
        /// </param>
        public void AddDeleteFormAction(MenuItemCollection menu)
        {
            var path = "/App_Plugins/formulate/menu-actions/deleteForm.html";
            var menuItem = new MenuItem()
            {
                Alias = "deleteForm",
                Icon = "folder",
                Name = "Delete Form"
            };
            menuItem.LaunchDialogView(path, "Delete Form");
            menu.Items.Add(menuItem);
        }


        /// <summary>
        /// Adds the "Move" action to the menu.
        /// </summary>
        /// <param name="menu">
        /// The menu items to add the action to.
        /// </param>
        public void AddMoveFormAction(MenuItemCollection menu)
        {
            var path = "/App_Plugins/formulate/menu-actions/moveForm.html";
            var menuItem = new MenuItem()
            {
                Alias = "moveForm",
                Icon = "folder",
                Name = "Move"
            };
            menuItem.LaunchDialogView(path, "Move Form");
            menu.Items.Add(menuItem);
        }


        /// <summary>
        /// Adds the "Create Form" action with the specified ID used
        /// as the parent for the form that is created.
        /// </summary>
        /// <param name="menu">
        /// The menu to add the action to.
        /// </param>
        /// <param name="entityId">
        /// The ID of the entity to create the form under.
        /// If null, the form will be created at the root.
        /// </param>
        public void AddCreateFormAction(MenuItemCollection menu,
            Guid? entityId = null)
        {
            var path = "/formulate/formulate/editForm/null";
            var menuItem = new MenuItem()
            {
                Alias = "createForm",
                Icon = "folder",
                Name = "Create Form"
            };
            if (entityId.HasValue)
            {
                path = path + "?under=" + GuidHelper.GetString(entityId.Value);
            }
            menuItem.NavigateToRoute(path);
            menu.Items.Add(menuItem);
        }

        #endregion

    }

}