// Copyright 2014 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace GoogleVR.HelloVR {
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Collider))]
    public class ObjectController : MonoBehaviour {
    private new Renderer renderer;
    private BinocularBehaviorScript binocular;
    public GameObject player;
    public Material inactiveMaterial;
    public Material gazedAtMaterial;
    public AudioClip narrationClip;
    public AudioSource narration;
    public Text text;
    void Start() {
      renderer = GetComponent<Renderer>();
      SetGazedAt(false);
        this.binocular = this.transform.GetComponentInParent<BinocularBehaviorScript>();
    }

    public void SetGazedAt(bool gazedAt)
    {
        if (gazedAt)
        {
            StartCoroutine("Stared");
        }
        else
        {
            StopCoroutine("Stared");
        }
        if (inactiveMaterial != null && gazedAtMaterial != null)
        {
            renderer.material = gazedAt ? gazedAtMaterial : inactiveMaterial;
            return;
        }
    }

    public void TeleportRandomly() {
      PlayerCrontroller playerController = player.GetComponent<PlayerCrontroller>();
      playerController.binocular.current = false;
      playerController.binocular = this.binocular;
      this.binocular.current = true;
      //Para teletransportar
      player.transform.position = this.transform.position - new Vector3(1.85f,0,1.85f);
      //Play audio at telport
      narration.PlayOneShot(narrationClip);
      SetGazedAt(false);
     }

    IEnumerator Stared()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            TeleportRandomly();
        }           
    }
  }
}
