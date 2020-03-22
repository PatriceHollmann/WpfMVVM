﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Linq;
using System.Windows;

namespace WpfAppJSON
{
    public interface ICloseable
    {
        event EventHandler CloseRequest;
    }
    public class EditUserViewModel : BindableBase, ICloseable
    {
        public EditUserViewModel Self => this;
        public event EventHandler CloseRequest;
        protected void RaiseCloseRequest()
        {
            var handler = CloseRequest;
            if (handler != null) 
                handler(this, EventArgs.Empty);
        }
        public DelegateCommand CommandClose { get; }
        public DelegateCommand AddPerson { get; }
        public DelegateCommand _LostFocusCommand;
        public DelegateCommand LostFocusCommand
        { get { 
                return _LostFocusCommand;
    } }
       
        public House SelectedHouse { get; set; }
        public User SelectedPerson { get; set; }
        public House EditHouse { get; set; }
        public User EditPerson { get; set; }
        public Phone newPhone = new Phone();
        public Phone[] Phones { get {
                return EditPerson.Phones.Concat(new[] { newPhone }).ToArray();
            } }
        public bool isNewUser { get; set; }
        public bool isNewHouse { get; set; }


        public EditUserViewModel(DataStore dataStore,User user=null,House house=null)
        {
            SelectedPerson = user ?? new User();
            SelectedHouse = house?? new House();
            EditHouse = new House
            {
                Address = SelectedHouse.Address,
                Flors = SelectedHouse.Flors,
                Name = SelectedHouse.Name,
                Type = SelectedHouse.Type,
                UserId = SelectedHouse.UserId,
            };
            EditPerson = new User
            {
                Id = SelectedPerson.Id,
                Name = SelectedPerson.Name,
                Login = SelectedPerson.Login,
                Group = SelectedPerson.Group,
                Phones = SelectedPerson.Phones
            };
            isNewUser = user == null;
            isNewHouse = house == null;
            AddPerson = new DelegateCommand(() =>
                {
                    if (isNewUser)
                    {
                        dataStore.Users.Add(EditPerson);
                    }
                    else
                    {
                        SelectedPerson.Id = EditPerson.Id;
                        SelectedPerson.Name = EditPerson.Name;
                        SelectedPerson.Login = EditPerson.Login;
                        SelectedPerson.Group = EditPerson.Group;
                        SelectedPerson.Phones = EditPerson.Phones;
                    }
                    if (isNewHouse)
                    {
                        if (EditHouse.isValid())
                        {
                            dataStore.Houses.Add(EditHouse);
                        }
                        RaiseCloseRequest();
                    }
                    else
                    {
                    if (SelectedHouse.isValid())
                    {
                        SelectedHouse.Address = EditHouse.Address;
                            SelectedHouse.Flors = EditHouse.Flors;
                            SelectedHouse.Name = EditHouse.Name;
                            SelectedHouse.Type = EditHouse.Type;
                            SelectedHouse.UserId = EditHouse.UserId;
                            RaiseCloseRequest();
                        }
                        else
                        {
                            MessageBox.Show("Данные дома не корректны.");
                        }
                    }
                });
            CommandClose = new DelegateCommand(() =>
            {
                RaiseCloseRequest();
            });
            _LostFocusCommand = new DelegateCommand(() =>
            {
                Console.WriteLine("");
              //  if (!= "" && ) {
               //     EditPerson.Phones.Append(newPhone);
               //     newPhone = new Phone();
               // }
            });
        }
    }
}
