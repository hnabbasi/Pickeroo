using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace Pickeroo
{
    public class FancyPicker : Picker
    {
        int previousIndex = -1;

        public FancyPicker ()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        void OnSelectedIndexChanged (object sender, EventArgs e)
        {
            if (previousIndex != SelectedIndex) 
            {
                previousIndex = SelectedIndex;
                SelectedItem = ItemsSource [SelectedIndex];
            }
        }

        #region Fields

        //Bindable property for the items source
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create (nameof (ItemsSource), typeof(IList), typeof(FancyPicker), defaultBindingMode: BindingMode.TwoWay, propertyChanged:OnItemsSourcePropertyChanged);

        //Bindable property for the selected item
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create (nameof (SelectedItem), typeof (object), typeof (FancyPicker), defaultBindingMode:BindingMode.TwoWay, propertyChanged: OnSelectedItemPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IList ItemsSource {
            get { return (IList)GetValue (ItemsSourceProperty); }
            set { SetValue (ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem {
            get { return GetValue (SelectedItemProperty); }
            set { SetValue (SelectedItemProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [items source property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourcePropertyChanged (BindableObject bindable, object value, object newValue)
        {
            var picker = (FancyPicker)bindable;
            var notifyCollection = newValue as INotifyCollectionChanged;
            if (notifyCollection != null) {
                notifyCollection.CollectionChanged += (sender, args) => {
                    if (args.NewItems != null) {
                        foreach (var newItem in args.NewItems) {
                            if(newItem is IPickerItem)
                                picker.Items.Add(((IPickerItem)newItem).DisplayText ?? "");
                            else
                                picker.Items.Add ((newItem ?? "").ToString());
                        }
                    }
                    if (args.OldItems != null) {
                        foreach (var oldItem in args.OldItems) {
                            if(oldItem is IPickerItem)
                                picker.Items.Remove (((IPickerItem)oldItem).DisplayText ?? oldItem.ToString());
                            else
                                picker.Items.Remove ((oldItem ?? "").ToString ());
                        }
                    }
                };
            }

            if (newValue == null)
                return;

            picker.Items.Clear ();

            foreach (var item in (IList)newValue) {
                if (item is IPickerItem)
                    picker.Items.Add (((IPickerItem)item).DisplayText ?? "");
                else
                    picker.Items.Add ((item ?? "").ToString ());
            }
        }

        /// <summary>
        /// Called when [selected item property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnSelectedItemPropertyChanged (BindableObject bindable, object value, object newValue)
        {
            var picker = (FancyPicker)bindable;
            if (picker.ItemsSource != null)
                picker.SelectedIndex = picker.ItemsSource.IndexOf (picker.SelectedItem);
        }

        #endregion
    }

    //public static class EnumerableExtensions
    //{
    //    /// <summary>
    //    /// Returns the index of the specified object in the collection.
    //    /// </summary>
    //    /// <param name="self">The self.</param>
    //    /// <param name="obj">The object.</param>
    //    /// <returns>If found returns index otherwise -1</returns>
    //    public static int IndexOf (this IList self, object obj)
    //    {
    //        //return self.IndexOf (obj);
    //        //int index = -1;

    //        //var enumerator = self.GetEnumerator ();
    //        //enumerator.Reset ();
    //        //int i = 0;
    //        //while (enumerator.MoveNext ()) {
    //        //    if (enumerator.Current == obj) {
    //        //        index = i;
    //        //        break;
    //        //    }

    //        //    i++;
    //        //}

    //        //return index;
    //    }
    //}
}
