using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BusinessAlgorithm.Test {

    public class DropdownElement : MonoBehaviour, IPointerClickHandler {
        public Dropdown[] otherDropDown;
        public void OnPointerClick(PointerEventData eventData) {
            foreach (var item in otherDropDown) {
                item.Hide();
            }
        }
    }
}
