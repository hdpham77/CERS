using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UPF.Windows {
	public static class ControlValidationExtensions {

		public static bool IsValid(this TextBox textBox, string fieldName, string tip, ControlValidationStateHandler controlValidationStateHandler, int minLength, int maxLength) {
			bool result = false;

			if (textBox.Text.Length >= minLength && textBox.Text.Length <= maxLength) {
				result = true;
			}

			HandleControlValidationState(textBox, fieldName, tip, result, controlValidationStateHandler);

			return result;
		}

		public static bool IsValid(this TextBox textBox, string fieldName, string tip, ControlValidationStateHandler controlValidationStateHandler, int minLength) {
			return IsValid(textBox, fieldName, tip, controlValidationStateHandler, minLength, int.MaxValue);
		}

		public static bool IsValid(this TextBox textBox, string fieldName, string tip, ControlValidationStateHandler controlValidationStateHandler) {
			return IsValid(textBox, fieldName, tip, controlValidationStateHandler, 1);
		}

		public static bool IsValid(this ComboBox comboBox, string fieldName, string tip, ControlValidationStateHandler controlValidationStateHandler) {
			bool result = false;

			if (comboBox.SelectedItem != null) {
				result = true;
			}

			HandleControlValidationState(comboBox, fieldName, tip, result, controlValidationStateHandler);


			return result;
		}

		public static bool IsValidTag(this Control control, string fieldName, string tip, ControlValidationStateHandler controlValidationStateHandler) {
			bool result = false;
			if (control.Tag != null) {
				result = true;
			}

			HandleControlValidationState(control, fieldName, tip, result, controlValidationStateHandler);

			return result;
		}

		public static void HandleControlValidationState(Control control, string fieldName, string tip, bool result, ControlValidationStateHandler controlValidationStateHandler) {
			if (controlValidationStateHandler != null) {
				string tempTip = null;

				if (!result) {
					if (tip == null) {
						tempTip = fieldName + " is required.";
					}
					else {
						tempTip = tip;
					}

				}
				controlValidationStateHandler(control, tempTip, result);
			}
		}

		public static bool IsNumeric(this TextBox textBox, string fieldName, string tip, long minimumValue, long maximumValue, ControlValidationStateHandler controlValidationStateHandler) {
			bool result = false;

			long value;
			if (long.TryParse(textBox.Text.Trim(), out value)) {
				if (value < maximumValue) {
					result = true;
				}
			}

			HandleControlValidationState(textBox, fieldName, tip, result, controlValidationStateHandler);

			return result;
		}

		public static bool IsShortValue(this TextBox textBox, string fieldName, ControlValidationStateHandler controlValidationStateHandler) {
			return IsNumeric(textBox, fieldName, "Value must be a number and less or equal to " + short.MaxValue + ".", short.MinValue, short.MaxValue, controlValidationStateHandler);
		}

	}
}
